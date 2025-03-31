[中文说明](./README.md)

# Asp.Net core mvc webapi demo

This is a demo project of the back-end of my vue3 front-end app demo. The functionality of this demo is identical to the minimal api style demo and the mvc webapi style demo (repositories in my account). The only difference from the other two is that using MongoDB for storage instead of Sql Server and Redis. It demonstrates how to use Wechat for login and jwt-token authentication.

## Configuration

See the /Models/EnvironmentConfig.cs file. Using the default config shoud be enough for development.

## Project Setup

```sh
dotnet restore
```

### Serve for Development

```sh
dotnet run dev
```

### Deploy for Production

This is a self contained console app, so just make it a system service/daemon and export the config to the system environment.
