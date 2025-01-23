## Getting Started with HelloPhotino.Vue
This sample demonstrates how to create a basic Vue starter app in a Photino.NET application. It utilizes Photino.NET.Server which serves (often bundled) UI files from a local ASP.NET server. This gets around issues with serving JavaScript modules, bundles, and other resources from static files and allows the UI to make standard service calls to the .NET back end if desired.

## Development
### User Interface
The entire user interface is contained in the UserInterface folder. It can be maintained independently of the .NET portion of the application. It uses standard Vue development paradigms. To begin development, first issue the following command from the UserInterface folder to install the necessary dependencies:
```bash
npm install
```

During development, the user is expected to run the Vue development server using the following command from the UserInterface folder:
```bash
npm run dev
``` 

### .NET
During development, The .NET application expects the Vue development server to be running (see above). The .NET application will proxy requests to the Vue development server. To run the .NET application, issue the following command from the UserInterface folder:
```bash
dotnet run
```
If the development server isn't running, the UI will not be able to serve the UI and you'll get a "localhost refused to connect" or similar error.

## Production
### User Interface
The first step in doing a production build of the UI is to issue the following command from the UserInterface folder:
```bash
npm run build
```
This will create a folder named dist in the UserInterface folder. Manually copy the contents of this folder to the wwwroot folder in the .NET project. This process is not automated because it differs by operating system. Feel free to automate this step in your own environment.

### .NET
After doing a production build of the user interface and copying the resulting output to the wwwroot folder, the .NET application can be built in release mode by using the Configuration Manager in Visual Studio or by issuing the following command from the root .NET project folder:
```bash
dotnet build -c Release
```

### Photino.NET.Server
In release mode, the UI is served from ASP.NET. Photino.NET.Server serves files from either embedded resources (built into the .NET assembly) or from the file system. In this sample, we placed the wwwroot folder in the Resources folder. All files in the Resources folder are embedded into the .NET assembly and don't need to be distributed separately. When run, the server will create a wwwroot folder structure in the output folder. Any files placed in these folders will override the embedded resources. Alternately, you may choose to move the wwwroot folder to the root of the project and set the files to be copied to the output folder. This is how many of the simpler examples are configured. Or you can use both methods. The choice is yours.