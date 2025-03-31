[English read me](./README.EN.md)

# Asp.Net core minimal api demo

这是我的Vue3前端Demo项目的配套后端。本项目的功能与我账号下的另外两个后端demo一致。展示了微信登录和使用jwt-token身份验证。唯一区别是这个demo使用MongoDB数据库，另外两个使用Sql Server和Redis。

## 服务器配置

详见/Models/EnvironmentConfig.cs文件。开发环境使用默认配置即可。

## 项目安装

```sh
dotnet restore
```

### 开发环境运行

```sh
dotnet run dev
```

### 生产环境部署

这是一个可独立运行的控制台程序，可使用system service/daemon部署，将配置参数写入系统环境即可。
