# Photino.React Hello World Sample

## In Debug mode

To run UI:

```sh
cd UserInterface && npm start
```

To run Photino Window:

```sh
dotnet run
```

## In Release mode

The Release mode uses the built React app, so to build UI and run Photino in one command, execute the following in a terminal:

```sh
# From inside root folder execute:
# `npm run build` will update the
# wwwroot folder after build is complete. 
cd UserInterface && npm run build && cd .. && dotnet run
```
