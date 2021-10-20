# Build native, cross-platform desktop apps

Photino is a lightweight open-source framework for building native,  
cross-platform desktop applications with Web UI technology.

Photino enables developers to use fast, natively compiled languages like C#, C++, Java and more. Use your favorite development frameworks like .NET 5, and build desktop apps with Web UI frameworks, like Blazor, React, Angular, Vue, etc.!

Photino uses the OSs built-in WebKit-based browser control for Windows, macOS and Linux.
Photino is the lightest cross-platform framework. Compared to Electron, a Photino app is up to 110 times smaller! And it uses far less system memory too!

# Samples

This repo contains samples for Photino projects using the following Web Frameworks:
  * MS Blazor
  * Vue.JS
  * Angular
  * React
  
This repo also contains these samples
  * Plain vanilla html/css/js.
  * WebAPI for local communication
  * gRPC for local communication
  * 3D graphics using 3d.js
  * 3D graphics with 3d.js and React
  * Advanced .NET with WebAPI, OS calls, PowerShell calls
  * Multi-Window (Linux and Mac only for now)

Contribute to this project if you would like to add additional support for frameworks currently not supported by Photino.
If you would like to start working with Photino and a particular supported framework, you can either start with using one of the sample projects, or check out the [Visual Studio Project Templates](https://tryphotino.kavadocs.com/Photino-VSExtension) or [dotnet CLI / VS Code tempaltes](https://tryphotino.kavadocs.com/Photino-VSCodeTemplates).

# Troubleshooting

If you experience issues building the sample projects, first make sure you have restored the required Photino Nuget Packages, and that you're building for x64, and not x86. 
If you are on Windows, make sure Microsoft Edge Dev is installed: https://www.microsoftedgeinsider.com/en-us/download.

