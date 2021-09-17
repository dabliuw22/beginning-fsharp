# beginning-fsharp

## install `libgdiplus` for `System.Drawing.Common`:
```bash
$ brew install mono-libgdiplus
```

## Create Project:
```bash
$ dotnet new console -lang F# -o src/App
```
## Dependencies:
```bash
$ dotnet paket add FSharp.Control.Reactive --version 5.0.2 --project App
$ dotnet paket add System.Drawing.Common --version 5.0.2 --project App
```

## Install:
```bash
$ dotnet paket install
```

## Run Project:
```bash
$ dotnet run --project src/App
```