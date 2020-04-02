# RESTfulAPITemplate
简体中文 | [English](./README.md)

> 基于.NET-CLI的项目模板，协助搭建符合客制化要求的REST风格的WEBAPI项目脚手架

## 依赖
* .netcore 3.1

## 脚手架中集成第三方类库
* [Automapper](https://automapper.org/)
* [FluentValidation](https://fluentvalidation.net/)
* [Serilog](https://serilog.net/)
* [Autowrapper](https://github.com/proudmonkey/AutoWrapper)
* [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
* [Marvin.Cache.Headers](https://github.com/KevinDockx/HttpCacheHeaders)
* [Larsson.RESTfulAPIHelper](https://github.com/larssonsun/Larsson.RESTfulAPIHelper)

## 安装（本说明均在PowerShell或者Cmder下使用.NET-CLI进行，下同）
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
新建一个目录，比如*demo*，并在该目录中使用模板新建一个项目
```
> mkdir demo
> cd demo
> dotnet new restful-api-l -o . -n templateUseDemo
```
如成功，会提示
```
The template "ASP.NET Core RESTfulAPI Template by Larsson" was created successfully.
```
您将在*demo*下得到如下结构的项目脚手架（这里只列出关键文件及目录，下同）

```
demo
├─templateUseDemo.Api - API服务项目
│  ├─Controllers - 控制器
│  ├─Middlewares - 中间件
│  └─Services - 外部服务，如JWT生成器（本模板为本地实现，实际可使用`IndentityServer4`等验证框架）
│  └─appsettings.json - 配置文
├─templateUseDemo.Core - 核心项目
│  ├─Configurations - 模型配置
│  │  ├─PropertyMappings - DTO与实例模型映射
│  │  ├─SortMappings - DTO中排序属性与模型的若干属性映射
│  │  └─Validators - DTO验证
│  ├─DomainModels - 其他领域模型
│  ├─DTOs - DTO
│  ├─Entities - 实例模型
│  └─Interfaces - 接口
└─templateUseDemo.Infrastructure - 基础设施项目
    └─Repositories - 数据仓储
```
进入*templateUseDemo.Api*目录，运行项目
```
> cd templateUseDemo.Api
> dotnet restore
> dotnet run
```
控制台得到如下结果表示成功
```
[11:29:29 INF] Now listening on: http://localhost:5000
[11:29:29 INF] Now listening on: https://localhost:5001
[11:29:29 INF] Application started. Press Ctrl+C to shut down.
[11:29:29 INF] Hosting environment: Production
[11:29:29 INF] Content root path: xxxx\demo\templateUseDemo.Api
```
## 参数
*-esfoss|--enable-support-for-obsolete-sql-server*
* 使用EF2.x以便使EF支持SQL Server 2012之前版本的数据库的翻页查询（ROW_NUMBER()），请查看 [issue](https://github.com/dotnet/efcore/issues/13959)。
* bool - Optional
* Default: false / (*) true

*-elrh|--enable-larsson-restfulapi-helper*
* 使用 `Larsson.RESTfulAPIHelper` 来简化结果数据局的排序，分页，塑形操作。
* bool - Optional
* Default: true

*-egrhw|--enable-global-response-handler-wrapper*
* 使用全局HTTP异常处理程序和响应包装器，例如给statuscode为200的response附加body。详见[`AutoWrapper`](https://github.com/proudmonkey/AutoWrapper)
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
## 数据库迁移及DataSeed
> 如果需要使用`MS-SQLServer`作为数据库存储媒介，则需要注意以下内容

新建目录并在该目录中创建带MS-SQLServer数据库的项目
```
> mkdir demoForDb
> cd demoForDb
> dotnet new restful-api-l -o . -n templateUseDemoForDb -dt MSSQL -esfoss true
```
执行成功
```
The template "ASP.NET Core RESTfulAPI Template by Larsson" was created successfully.
```
您将在目录*demoForDb*下得到如下结构的项目脚手架（多了一个迁移项目）

```
demoForDb
├─templateUseDemoForDb.Api
│  ├─Controllers
│  ├─Middlewares
│  └─Services
│  └─appsettings.json
├─templateUseDemoForDb.Core
│  ├─Configurations
│  │  ├─PropertyMappings
│  │  ├─SortMappings
│  │  └─Validators
│  ├─DomainModels
│  ├─DTOs
│  ├─Entities
│  └─Interfaces
├─templateUseDemoForDb.EfMigration - 数据库迁移项目
│  ├─DbContext - 数据库上下文
│  ├─EntityConfigurations - 使用Fluent API方式配置模型映射（从DBcontext中独立出来）
│  └─Migrations - 迁移文件（由迁移自动生成）
│  └─appsettings.json - 配置文件(数据库连接部分需与API一致)
│  └─DemoContextSeed.cs - 数据种子文件（提供示例数据）
└─templateUseDemoForDb.Infrastructure
    └─Repositories
```

进入项目*templateUseDemoForDb.EfMigration*的目录并修改配置文件*appsettings.json*中的数据库连接
```
> cd templateUseDemoForDb.EfMigration
> notepad appsettings.json
```
```json
...
"ConnectionStrings": {
  "templateUseDemoForDbDbConnStr": "（修改为你的数据库连接字符串）"
},
...
```
运行数据库迁移项目
```
> dotnet restore
> dotnet run
```
稍等片刻，得到如下结果表示成功（没有任何错误或者警告的话）
```
...
若干种子数据被添加（如果是第一次执行的话）
...
[10:23:40 INF] Seed data created.
[10:23:40 DBG] 'DemoContext' disposed.
[10:23:40 INF] **Data migration completed.**
```
再进入api项目*templateUseDemoForDb.Api*并修改配置文件*appsettings.json*中的数据库连接（与刚才相同）
然后运行api项目
```
> dotnet restore
> dotnet run
```
稍等片刻，得到如下结果表示成功
```
[16:50:07 INF] Now listening on: http://localhost:5000
[16:50:07 INF] Now listening on: https://localhost:5001
[16:50:07 INF] Application started. Press Ctrl+C to shut down.
[16:50:07 INF] Hosting environment: Production
[16:50:07 INF] Content root path: xxxxx\templateusedemofordb.api
```

## 本模板项目遵循协议
[Apache License 2.0](https://github.com/larssonsun/RESTfulAPITemplate/blob/master/LICENSE) license.

Copyright (c) 2020-present Larssonsun