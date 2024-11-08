## Getting Started with HelloPhotino.3d.React Sample
This sample demonstrates how to create a 3D Photino.NET application using React and Three.js. The sample uses the Photino.NET framework to create a desktop application that hosts a React application that uses Three.js to render a 3D scene.

## Development
### User Interface
The entire user interface is contained in the UserInterface folder. It can be maintained independently of the .NET portion of the application. It uses standard React development paradigms. To begin development, first issue the following command from the UserInterface folder to install the necessary dependencies:
```bash
npm install
```

During development, the user is expected to run the React development server using the following command from the UserInterface folder:
```bash
npm run start
```

### .NET
During development, The .NET application expects the React development server to be running (see above). The .NET application will proxy requests to the React development server. To run the .NET application, issue the following command from the UserInterface folder:
```bash
dotnet run
```

## Production
### User Interface
The first step in doing a production build of the UI is to issue the following command from the UserInterface folder:
```bash
npm run build
```
This will create a folder named build in the UserInterface folder. Manually copy the contents of this folder to the wwwroot folder in the .NET project. This process is not automated because it differs by operating system. Feel free to automate this step in your own environment.

### .NET
After doing a production build of the user interface and copying the resulting output to the wwwroot folder, the .NET application can be built in release mode by using the Configuration Manager in Visual Studio or by issuing the following command from the root .NET project folder:
```bash
dotnet build -c Release
```