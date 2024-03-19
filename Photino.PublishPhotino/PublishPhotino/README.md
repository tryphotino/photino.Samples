# Cross-Platform Publishing with Photino.NET: A Comprehensive Guide

Developing applications that are seamlessly functional across all major operating systems is a significant time-saver in software development. However, the process of deploying these applications introduces a whole new set of challenges. This comprehensive guide is designed to demystify the process of publishing your Photino.NET applications on Windows, macOS, and Linux.

## TL;DR

You can use this project as a starting point for your Photino.NET application. It contains a `*.csproj` file with the necessary settings to create a single file executable for Windows, macOS, and Linux. You can modify and run the `publish.sh` script to create packages for Windows, macOS, and Linux.

```bash
# Chnge dir to `publish`
cd publish
# Run the publish script
./publish.sh
```

The resulting files will be placed in the `publish/output` directory. You can then use these files to distribute your application.

> Please use WSL on Windows to run the `publish.sh` script. The script uses Unix shell commands and will not work on the Windows Command Prompt or PowerShell.

## Publishing is confusing, frustrating and cumbersome

Publishing applications across different platforms is not a straightforward task. It involves understanding the packaging, distribution, and dependencies management for each operating system. This guide builds upon the foundation laid by the practical experiences of deploying an internal tool developed with Photino.NET, offering insights and strategies applicable to a broader scope of .NET applications.

I had to limit the scope of this guide to publishing for the most common operating systems, if not just to keep my own sanity. Especially Linux has a wide variety of distributions and package managers, and I can't cover them all. But I will provide a starting point for you to find the right package manager for your application.

## Here, just double click

Maybe it's just me, but if at all possible I want to send a link to a co-worker and tell them to
unpack the file and double click ... **"it just works"™** ... I blame Apple for expecting this behavior.

This comes with certain drawbacks though, as installers fulfill important tasks like adding start menu shortcuts and such.

You may get away doing some of these task on (first) startup of your app. But especially on Linux you will be limited in how you can resolve dependencies which gets complicated quickly. Due to the variety of ways to create installers and packages on Linux I limit this guide to representative examples, deb packages and Flatpak.

### Pre-requisites: A Quick Primer

Before diving into the details, ensure you have a basic understanding of Photino.NET, the .NET Command-Line Interface (CLI), and the significance of cross-platform compatibility. Familiarity with these tools and concepts will help you follow the subsequent sections with ease.

For readability's sake I will show all examples using the .NET CLI and Unix Shell commands. But equivalent commands and procedures can be used for Visual Studio and Powershell as well.

## Publishing Photino.NET applications

After you created a new project using one of the Photino templates, your `*.csproj` file will look something like Listing 1 below. Based on this we'll explore how to configure and publish your application for each target platform.

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Photino.NET" Version="2.6.0" />
  </ItemGroup>

  <ItemGroup>
    <Content Include="wwwroot/**" CopyToOutputDirectory="PreserveNewest" />
  </ItemGroup>

</Project>
```

> Listing 1: A typical Photino.NET project file

If we publish the project using these settings with `dotnet publish`, we generate a publish folder with the contents as seen in Listing 2.

```
bin/Release/net8.0/publish
├── Photino.NET.dll
├── PublishPhotino
├── PublishPhotino.deps.json
├── PublishPhotino.dll
├── PublishPhotino.pdb
├── PublishPhotino.runtimeconfig.json
├── runtimes
│   ├── linux-arm64
│   │   └── native
│   │       └── Photino.Native.so
│   ├── linux-x64
│   │   └── native
│   │       └── Photino.Native.so
│   ├── osx-arm64
│   │   └── native
│   │       └── Photino.Native.dylib
│   ├── osx-x64
│   │   └── native
│   │       └── Photino.Native.dylib
│   ├── win-arm64
│   │   └── native
│   │       ├── Photino.Native.dll
│   │       └── WebView2Loader.dll
│   └── win-x64
│       └── native
│           ├── Photino.Native.dll
│           └── WebView2Loader.dll
└── wwwroot
    └── index.html
```

> Listing 2: Publish folder contents after running `dotnet publish`

This is quite far from the single file executable we want to achieve. We can change this by adding a few settings to the `*.csproj` file as seen in Listing 3.

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <!--
      General properties
    -->
    <OutputType>WinExe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>

    <!--
      Publish properties
    -->
    <!-- Version will be set to a debug version if not overridden by CLI parameter. -->
    <Version>0.0.0-$([System.DateTime]::Now.ToString(yyyyMMddHms))</Version>

    <!-- Bundle the application with the specified .NET runtime -->
    <SelfContained>true</SelfContained>

    <!-- Bundle all dependencies into a single executable -->
    <PublishSingleFile>true</PublishSingleFile>
    <IncludeAllContentForSelfExtract>true</IncludeAllContentForSelfExtract>

    <!-- Enable compression for the single executable -->
    <EnableCompressionInSingleFile>true</EnableCompressionInSingleFile>

    <!--
    Disable default content for better control of which files are bundled.
    Use the <Content> item to include files in the bundle, or enable these properties to include all files.
    @See: https://docs.microsoft.com/en-us/dotnet/core/deploying/single-file#default-content-in-single-file-bundles
    -->
    <EnableDefaultContent>false</EnableDefaultContent>
    <EnableDefaultContentItems>false</EnableDefaultContentItems>

    <!-- Include debug symbols into the executable -->
    <DebugType>embedded</DebugType>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Photino.NET" Version="2.6.0" />
  </ItemGroup>

  <ItemGroup>
    <Content Include="wwwroot/**" CopyToOutputDirectory="PreserveNewest" />
  </ItemGroup>

</Project>
```

> Listing 3: Adding publish properties to the `*.csproj` file

Much better, we're now left with a single, compressed executable as seen in Listing 4.

```
bin/Release/net8.0/osx-arm64/publish
└── PublishPhotino
```

> Listing 4: Publish folder contents after running `dotnet publish -r osx-arm64`

You might have noticed that the output path has changed from `bin/Release/net8.0/publish` to `bin/Release/net8.0/osx-arm64/publish`. This is because we enabled self-contained deployment, which means the system specific .NET runtime is included in the executable. To keep the file size for distribution down, the executable will be compressed since we enabled compression for the single file as well.

### Behavior of the single file executable

When you run the single file executable for the first time, it will extract the .NET runtime and all dependencies into a temporary directory and run the application from there. This means that the first run of the application will take a bit longer than subsequent runs, as the .NET runtime and dependencies need to be extracted.

On Windows you will find these files in the `%TEMP%/.net` directory, on macOS and Linux in the `$HOME/.net` directory. Mind that for each new release of your application, a new directory will be created in the temporary directory. In order to keep your users' machines clean, you should consider cleaning up these directories programmatically.

After running the example for this article, .NET extracted the files to `~/.net/PublishPhotino/Mj6TEwnRUV5cnbe_hy98ScbqoiijdwE=` on my macOS machine. The name of the directory will be different on each machine and for each new release of your application.

Amongst the extracted libraries and the PublishPhotino executable, you will also find the `wwwroot` directory we included with `<Content Include="wwwroot/**" CopyToOutputDirectory="PreserveNewest" />` in the `*.csproj` file. This may be helpful to know for debugging purposes. To add further files to the extracted directory, simply add them in the same way.

For more information on how to publish single file executables reference the [Single File Deployment Docs](https://learn.microsoft.com/en-us/dotnet/core/deploying/single-file/overview).

## Deploying for Windows

Publishing for Windows is as simple as running `dotnet publish -r win-x64` with the runtime parameter set to the "Runtime Identifier" (rid) set to `win-x64`. You can use `win-x86` and `win-arm64` to target these specific platforms. This will create a single file executable for Windows as seen in Listing 5.

```
bin
|- Release / net8.0 / win-x64 / publish
   |  PublishPhotino.exe
```

> Listing 5: Publish folder contents after running `dotnet publish -r win-x64`

A user can now run the `PublishPhotino.exe` file on their Windows machine without having to install anything else.

**Note:** Depending on your application you might need to create an installer to fulfill certain tasks, like adding a shortcut to the start menu or desktop. For this you can use tools like the [WiX Toolset](https://wixtoolset.org/) or [Inno Setup](https://jrsoftware.org/isinfo.php).

## Deploying for macOS

To deploy a Photino application for macOS you will run `dotnet publish -r osx-x64` and also package the application into an `.app` bundle. This bundle is a directory that contains the application executable, the required libraries, and metadata about the application.

The minimal structure for a macOS application bundle is shown in Listing 6. The `PublishPhotino` executable is placed in the `macOS` directory, and the `Info.plist` file contains metadata about the application. The `BundleIcon.png` file is the icon that will be displayed in the Finder.

You can simply copy the `PublishPhotino` executable from Listing 4 into the `macOS` directory and create the `Info.plist` and `BundleIcon.png` files yourself.

```
PublishPhotino.app
└── Contents
    ├── Info.plist
    ├── MacOS
    │   └── PublishPhotino
    └── Resources
        └── BundleIcon.png
```

> Listing 6: App bundle contents for macOS application

The `Info.plist` file should contain the content as seen in Listing 7:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
  <key>CFBundleExecutable</key>
  <string>PublishPhotino</string>
  <key>CFBundleIconFile</key>
  <string>BundleIcon.png</string>
  <key>CFBundleIdentifier</key>
  <string>io.tryphotino.PublishPhotino</string>
  <key>CFBundleName</key>
  <string>PublishPhotino</string>
  <key>CFBundlePackageType</key>
  <string>APPL</string>
  <key>CFBundleVersion</key>
  <string>1.0</string>
  <key>LSMinimumSystemVersion</key>
  <string>13.0</string>
</dict>
</plist>
```

> Listing 7: Info.plist file for macOS application

There are some more keys you can add to the `Info.plist` file, like `CFBundleDisplayName` and `CFBundleShortVersionString`. For a full list of keys and their meanings, refer to the [Apple Developer Documentation](https://developer.apple.com/documentation/bundleresources/information_property_list).

For Webcam and Microphone access, the following keys (Listing 8) need to be added to the `Info.plist` file:

```xml
<key>NSCameraUsageDescription</key>
<string>Camera access is required to take pictures.</string>
<key>NSMicrophoneUsageDescription</key>
<string>Microphone access is required to record audio.</string>
```

> Listing 8: Info.plist file with camera and microphone access keys

[NsCameraUsageDescription](https://developer.apple.com/documentation/bundleresources/information_property_list/nscamerausagedescription) and [NsMicrophoneUsageDescription](https://developer.apple.com/documentation/bundleresources/information_property_list/nsmicrophoneusagedescription) are required to access the camera and microphone on macOS.

For distribution on the App Store you will need to sign your application with a valid Apple Developer ID. For more information on how to sign your application, refer to the [Apple Developer Documentation](https://developer.apple.com/documentation/xcode/notarizing_macos_software_before_distribution).

## Deploying for Linux

Using the file that is created when running `dotnet publish -r linux-x64` directly **can work** on the more common Linux distributions. However it is recommended to package your application using an applicable package manager for the target distribution.

### Packaging for Debian-based distributions

As an example, to bundle a `.deb` package, you create a directory structure as seen in Listing 9. You then place the `PublishPhotino` executable you created with `dotnet publish` into the `bin` directory. The rest of the files are either copied or generated inside the respective directories.

You can for example use the `git log` to genereate the `changelog.gz` file on the fly when you create the package.

```
debian-build
├── DEBIAN
│   └── control
├── bin
│   └── PublishPhotino
└── usr
    └── share
        ├── applications
        │   └── PublishPhotino.desktop
        ├── doc
        │   └── PublishPhotino
        │       ├── changelog.gz
        │       ├── copyright
        │       └── README
        └── icons
            └── hicolor
                └── 512x512
                    └── apps
                        └── PublishPhotino.png
```

> Listing 9: Directory structure for `debian-build` directory

### The `control` file

The `control` file with the content as seen in Listing 10 will be used when installing the `.deb` package to provide metadata about the package and the dependencies. Photino requires certain, commonly found dependencies. Despite them being commonly installed already, you will want to make sure that these are installed on the target system. The dependencies as listed in Listing 10, are valid for Photino.NET 2.6.0.

```
Source: PublishPhotino
Section: custom
Priority: optional
Maintainer: your name <yourname@example.com>
Package: PublishPhotino
Version: 1.0.0
Architecture: amd64
Depends: libc6 (>= 2.31-13), gir1.2-gtk-3.0 (>= 3.24.24-4), libwebkit2gtk-4.0-37 (>= 2.42.4-0)
Description: PublishPhotino
  A simple Photino application.
```

> Listing 10: control file for .deb package. Replace values as needed.

### The 'desktop' file

The `PublishPhotino.desktop` file is used to create a shortcut in the start menu. The file should contain the content as seen in Listing 11.

```ini
[Desktop Entry]
Name=PublishPhotino
Comment=A simple Photino application
Exec=/usr/bin/PublishPhotino
Icon=/usr/share/icons/hicolor/512x512/apps/PublishPhotino.png
Terminal=false
Type=Application
Categories=Utility;
```

> Listing 11: desktop file for .deb package

The directories and files in Listing 9 will have to be set to be owned by root. Then you can create and rename the package as seen in Listing 12.

```bash
# Create the package
dpkg-deb --root-owner-group --build ./deb-build

# Rename the package
mv ./deb-build.deb ./PublishPhotino.deb
```

> Listing 12: Create and rename the .deb package

The resulting file can be installed using `apt install ./PublishPhotino.deb`, or using a package manager that supports `.deb` packages, like dpgk, Synaptic or GDebi.

### Universal packaging with Flatpak

Flatpak is a package manager for Linux that allows you to package your application and its dependencies into a single package. This package can then be installed on any Linux distribution that supports Flatpak.

It is required for the developer AND for the user to have Flatpak installed. You can find the installation instructions for Flatpak on the Flatpak website (https://flatpak.org/setup/). For systems that use dpkg / apt as package managers you can use the command in Listing 13 to install Flatpak.

```bash
# Install Flatpak
sudo apt install flatpak

# Add the Flathub repository
flatpak remote-add --if-not-exists flathub https://dl.flathub.org/repo/flathub.flatpakrepo
```

> Listing 13: Install Flatpak and add Flathub repository

This will allow Flatpak to resolve dependencies like the runtime you specify in the manifest file. In our case this is `org.gnome.Platform`. When a user of your Photino app installs the Flatpak package, Flatpak will install the runtime if it is not yet installed.

The required file structure for a Flatpak package is similar to the one for a `.deb` package. See Listing 13.

```
flatpak-build
├── app
│   └── share
│       ├── applications
│       │   └── io.tryphotino.publishphotino.desktop
│       ├── doc
│       │   └── io.tryphotino.publishphotino
│       │       ├── changelog.gz
│       │       ├── copyright
│       │       └── README
│       └── icons
│           └── hicolor
│               └── 512x512
│                   └── apps
│                       └──  io.tryphotino.publishphotino.png
└── bin
    └── PublishPhotino
io.tryphotino.publishphotino.yml
```

> Listing 13: Directory structure `flatpak-build` directory

Match the filenames for the manifest, .desktop and icon files with the id in your manifest file.

To package your Photino application as a Flatpak, you will need to create a `flatpak-builder` manifest file as seen in Listing 14. The manifest file needs to be named after the Flatpak `id` of your application, in our case `io.tryphotino.publishphotino`.

```yaml
id: io.tryphotino.publishphotino
runtime: org.gnome.Platform
runtime-version: "45"
sdk: org.gnome.Sdk
command: PublishPhotino
modules:
    - name: publishphotino
      buildsystem: simple
      build-commands:
          - install -D PublishPhotino /app/bin/PublishPhotino
      sources:
          - type: file
            path: ./bin/PublishPhotino
finish-args:
    - "--socket=x11"
    - "--socket=wayland"
    - "--share=ipc"
    - "--filesystem=home"
```

> Listing 14: Flatpak manifest file for Photino application

The `id` is a unique identifier for your application. The `runtime` and `sdk` are the runtime and SDK that your application will use. The `command` is the command that will be run when the application is started. The `finish-args` define the permissions that your application will have. The `modules` section contains the information about your application.

The Flatpak package can then be build and bundled into an installation file as seen in Listing 15.

```bash
# Build the Flatpak
flatpak-builder --force-clean --repo=./flatpak-repo ./flatpak-build io.tryphotino.publishphotino.yml

# Create the Flatpak installation file
flatpak build-bundle ./flatpak-repo PublishPhotino.flatpak io.tryphotino.publishphotino
```

> Listing 15: Build and bundle the Flatpak package

The resulting file can be installed using `flatpak install PublishPhotino.flatpak`, or using a package manager that supports Flatpak, like GNOME Software or KDE Discover. The user can then run the application using the command `flatpak run io.tryphotino.publishphotino`. Or start it from the start menu.

> Consider automating the creation of deb control, flatpak manifest and doc files for Linux packages. E. g. by replacing markers in these files with the current version of your application.

### Other packaging options

There are many other package managers for Linux, like Snap, AppImage, and RPM. Each of these package managers has its own way of packaging applications. For more information on how to package your application for these package managers, refer to their respective documentation.

Here's a starting point for knowing your package managers, list of Software Package Management Systems (https://en.wikipedia.org/wiki/List_of_software_package_management_systems).

-   To get started with Snap, you can use the Snapcraft tool (https://snapcraft.io/)
-   For AppImage, you can use the AppImageKit (https://appimage.org/).
-   For RPM, you can use the RPM Package Manager (https://rpm.org/).

---

![xkcd.com/927](https://imgs.xkcd.com/comics/standards.png)

> https://xkcd.com/927/\
> How standards proliferate.

## Conclusion

Cross-platform publishing, while challenging, offers immense rewards in terms of reach and user engagement. By adhering to the guidelines and best practices outlined in this guide, you'll be well-equipped to deploy your Photino.NET applications across Windows, macOS, and Linux with confidence. Happy coding, and may your applications run seamlessly across all platforms!

Remember, the journey of software development and deployment is a continuous learning process. Stay curious, experiment, and never hesitate to seek out new strategies and insights.
