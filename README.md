# Larsson.Template.RESTfulAPI
English | [简体中文](./README-zh.md)
> 基于.NET-CLI的项目模板，协助搭建符合客制化要求的项目脚手架
## 依赖
* .netcore 3.1

## 安装
```
> dotnet new --install Larsson.Template.RESTfulAPI
```
验证模板的安装
```
> dotnet new --list
```
如果如下模板被新增则标识安装成功
Templates|Short Name|Language|Tags
-- | -- | -- | --
ASP.NET Core RESTfulAPI Template by Larsson|restful-api-l|[C#]|Web/WebAPI/RESTfulAPI
## 快速开始
新建一个目录，比如*demo*，在目录中使用.Net命令行工具
```
> mkdir demo
> cd demo
> dotnet new restful-api-l -o . -n templateUseDemo
```
您将在demo下得到如下结构的项目脚手架
```
demo
├─templateUseDemo.Api
│  ├─Controllers
│  ├─Middlewares
│  └─Services
├─templateUseDemo.Core
│  ├─Configurations
│  │  ├─PropertyMappings
│  │  ├─SortMappings
│  │  └─Validators
│  ├─DomainModels
│  ├─DTOs
│  ├─Entities
│  └─Interfaces
└─templateUseDemo.Infrastructure
    └─Repositories
```
## 参数
*-esfoss|--enable-support-for-obsolete-sql-server*
* 使用EF2.x以便使EF支持SQL Server 2012之前版本的数据库的翻页查询，请查看 [issue](https://github.com/dotnet/efcore/issues/13959)。
* bool - Optional
* Default: false / (*) true

*-elrh|--enable-larsson-restfulapi-helper*
* 使用 `Larsson.RESTfulAPIHelper` 来简化结果数据局的排序，分页，塑形操作。
* bool - Optional
* Default: true

*-egrhw|--enable-global-response-handler-wrapper*
* 使用全局HTTP异常处理程序和响应包装器，例如给statuscode为200的response附加body。
* bool - Optional
* Default: true

*-es|--enable-swagger*
* 集成swagger
* bool - Optional
* Default: false / (*) true

*-eja|--enable-jwt-authentication*
* 使用脚手架预设的验证方式（JWT）
* bool - Optional
* Default: true

*-erc|--enable-response-cache*
* 使用HTTP响应缓存
* bool - Optional
* Default: false / (*) true

*-ct|--cache-type*
* 预制缓存示例
* LocalMemoryCache    - 本地内存缓存
* DistributedCache    - 分布式缓存 (默认是本地内存，需要可自己集成redis)

*-dt|--db-type*
* 使用的数据库类型
* DbInMemory    - 内存数据库
* MsSQL         - MS-SQLServer
* Default: DbInMemory
## 数据库迁移及dataseed
> 如果需要使用MS-SQLServer作为数据库存储媒介，则需要注意以下内容

使用.Net命令行工具新建一个目录并创建项目
```
> mkdir demoForDb
> cd demoForDb
> dotnet new restful-api-l -o . -n templateUseDemoForDb -dt MSSQL -esfoss true
```
您将在demoForDb下得到如下结构的项目脚手架
```
demoForDb
├─templateUseDemoForDb.Api
│  ├─Controllers
│  ├─Middlewares
│  └─Services
├─templateUseDemoForDb.Core
│  ├─Configurations
│  │  ├─PropertyMappings
│  │  ├─SortMappings
│  │  └─Validators
│  ├─DomainModels
│  ├─DTOs
│  ├─Entities
│  └─Interfaces
├─templateUseDemoForDb.EfMigration
│  ├─DbContext
│  ├─EntityConfigurations
│  └─Migrations
└─templateUseDemoForDb.Infrastructure
    └─Repositories
```
进入项目*emplateUseDemoForDb.EfMigration*的目录（数据库迁移项目）
```
cd templateUseDemoForDb.EfMigration
```
修改配置文件*appsettings.json*中的数据库连接
```
> notepad appsettings.json
```
```json
...
"ConnectionStrings": {
  "templateUseDemoForDbDbConnStr": "你的数据库连接字符串"
},
...
```
运行数据库迁移项目
```
> dotnet run
```
稍等片刻，得到如下结果表示成功
```
[16:44:40 INF] Now listening on: http://localhost:5000
[16:44:40 INF] Now listening on: https://localhost:5001
[16:44:40 INF] Application started. Press Ctrl+C to shut down.
[16:44:40 INF] Hosting environment: Production
[16:44:40 INF] Content root path: xxxxx\templateUseDemoForDb.EfMigration
[16:44:40 DBG] Hosting started
```
Ctrl+C停止。再进入api项目*templateUseDemoForDb.Api*并修改配置文件*appsettings.json*中的数据库连接（与刚才相同）
然后运行api项目
```
> dotnet run
```
稍等片刻，得到如下结果表示成功
```
16:50:07 INF] Now listening on: http://localhost:5000
16:50:07 INF] Now listening on: https://localhost:5001
16:50:07 INF] Application started. Press Ctrl+C to shut down.
16:50:07 INF] Hosting environment: Production
16:50:07 INF] Content root path: xxxxx\templateusedemofordb.api
```

## 协议
[Apache License 2.0](https://github.com/larssonsun/RESTfulAPITemplate/blob/master/LICENSE) license.

Copyright (c) 2020-present Larssonsun