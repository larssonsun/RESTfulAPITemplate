# RESTfulAPITemplate
English | [简体中文](./README-zh.md)

> Project template based on .NET-CLI to help build RESTful web api project scaffolding that meets custom requirements
## Dependence
* .netcore 3.1

## Integrate third-party class libraries in scaffolding
* [Automapper](https://automapper.org/)
* [FluentValidation](https://fluentvalidation.net/)
* [Serilog](https://serilog.net/)
* [Autowrapper](https://github.com/proudmonkey/AutoWrapper)
* [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
* [Marvin.Cache.Headers](https://github.com/KevinDockx/HttpCacheHeaders)
* [Larsson.RESTfulAPIHelper](https://github.com/larssonsun/Larsson.RESTfulAPIHelper)

## Installation (These instructions are performed in PowerShell or Cmder using .NET-CLI, the same below)
```
dotnet new --install Larsson.Template.RESTfulAPI
```
Verify template installation
```
dotnet new --list
```
If the following template is added, the installation is successful.
Templates | Short Name | Language | Tags
-|-|-|-
ASP.NET Core RESTfulAPI Template by Larsson | restful-api-l | [C #] | Web / WebAPI / RESTfulAPI
## Quick start
Create a new directory, such as *demo*, and use the template to create a new project in this directory
```
> mkdir demo
> cd demo
> dotnet new restful-api-l -o. -n templateUseDemo
```
If successful, will prompt
```
The template "ASP.NET Core RESTfulAPI Template by Larsson" was created successfully.
```
You will get the following structure of the project scaffolding under*demo*(here only the key files and directories are listed, the same below)

```
demo
├─templateUseDemo.Api - API service items
│ ├─Controllers - Controller
│ ├─Middlewares - Middlewares
│ └─Services - External services, such as JWT generator (this template is a local implementation, you can actually use the authentication framework such as `IndentityServer4`)
│ └─appsettings.json - Configuration file
├─templateUseDemo.Core - Core project
│ ├─Configurations - Model configuration
│ │ ├─PropertyMappings - DTO and instance model mapping
│ │ ├─SortMappings - DTO Sorting attributes and some attribute mapping of the model
│ │ └─Validators - DTO
│ ├─DomainModels - Other domain models
│ ├─DTOs - DTO
│ ├─Entities - Example model
│ └─Interfaces - Interface
└─templateUseDemo.Infrastructure - Infrastructure Project
    └─Repositories - Data repositories
```
Go to the *templateUseDemo.Api* directory and run the project
```
> cd templateUseDemo.Api
> dotnet restore
> dotnet run
```
The console gets the following results indicating success
```
[11:29:29 INF] Now listening on: http: // localhost: 5000
[11:29:29 INF] Now listening on: https: // localhost: 5001
[11:29:29 INF] Application started. Press Ctrl + C to shut down.
[11:29:29 INF] Hosting environment: Production
[11:29:29 INF] Content root path: xxxx \ demo \ templateUseDemo.Api
```
## parameter
*-erpie|--enable-rownumber-pagination-in-ef3*
* Use ROW_NUMBER() in EF3.x for paging query (custom extension replaces the method in EF3.X)
* bool - Optional
* Default: true / (*) false

*-esfoss | --enable-support-for-obsolete-sql-server*
* Use EF2.x to enable EF to support page turning query (ROW_NUMBER ()) for databases before SQL Server 2012, please see [issue](https://github.com/dotnet/efcore/issues/13959).
* bool-Optional
* Default: false / (*) true

*-elrh | --enable-larsson-restfulapi-helper*
* Use `Larsson.RESTfulAPIHelper` to simplify sorting, pagination, and shaping of the result data bureau.
* bool-Optional
* Default: true

*-egrhw | --enable-global-response-handler-wrapper*
* Use global HTTP exception handlers and response wrappers, such as attaching a body to a response with a statuscode of 200. See [`AutoWrapper`](https://github.com/proudmonkey/AutoWrapper) for details
* bool-Optional
* Default: true

*-es | --enable-swagger*
* Integrated swagger
* bool-Optional
* Default: false / (*) true

*-eja | --enable-jwt-authentication*
* Use scaffolding preset verification method (JWT)
* bool-Optional
* Default: true

*-erc | --enable-response-cache*
* Use HTTP response cache
* bool-Optional
* Default: false / (*) true

*-ct | --cache-type*
* Pre-made cache example
* LocalMemoryCache-local memory cache
* DistributedCache-distributed cache (default is local memory, you need to integrate redis yourself)

*-dt | --db-type*
* Type of database used
* DbInMemory-In-memory database
* MsSQL-MS-SQLServer
* Default: DbInMemory

*-ec | --enable-consul*
* Use Consul as a microservice discovery
* bool - Optional
* Default: false / (*) true

## Database Migration and DataSeed
> If you need to use `MS-SQLServer` as the database storage medium, you need to pay attention to the following

Create a new directory and create a project with MS-SQLServer database in the directory
```
> mkdir demoForDb
> cd demoForDb
> dotnet new restful-api-l -o. -n templateUseDemoForDb -dt MSSQL -esfoss true
```
execution succeed
```
The template "ASP.NET Core RESTfulAPI Template by Larsson" was created successfully.
```
You will get a project scaffolding with the following structure under the directory*demoForDb*(an additional migration project)

```
demoForDb
├─templateUseDemoForDb.Api - application layer
│ ├─Configurations
│ │ ├─PropertyMappings - Configure the mapping between DTO and entity, Filter and Command
│ │ ├─SortMappings - Configure sorting based on the mapping of specified attributes in DTO to attributes in entities
│ │ └─Validators - Configure DTO validation
│ ├─Controllers
│ ├─Middlewares
│ ├─Services-Application Service
│ └─appsettings.json
│
├─templateUseDemoForDb.Core - domain layer
│ ├─Entities - domain entities (aggregation, entity, value object)
│ ├─Interfaces
│ ├─SeedWork
│ ├─Services - Domain Services
│ └─Specifications - Domain Rules
│
├─templateUseDemoForDb.EfMigration - database migration project
│ ├─DbContext - database context
│ ├─EntityConfigurations - Use Fluent API to configure model mapping (independent from DBcontext)
│ ├─Migrations - migration file (automatically generated by migration)
│ ├─appsettings.json - configuration file (database connection part must be consistent with API)
│ └─DemoContextSeed.cs - data seed file (provide sample data)
│
└─templateUseDemoForDb.Infrastructure - Infrastructure layer
  ├─Repositories - storage
  └─Utils - Infrastructure Services
```

Go to the directory of the project*templateUseDemoForDb.EfMigration*and modify the database connection in the configuration file*appsettings.json*
```
> cd templateUseDemoForDb.EfMigration
> notepad appsettings.json
```
```json
...
"ConnectionStrings": {
  "templateUseDemoForDbDbConnStr": "(Modify to your database connection string)"
},
...
```
Run the database migration project
```
> dotnet run
```
Wait for a while and get the following result to indicate success (without any errors or warnings)
```
...
Several seed data are added (if this is the first execution)
...
[10:23:40 INF] Seed data created.
[10:23:40 DBG] 'DemoContext' disposed.
[10:23:40 INF]**Data migration completed.**
```
Then enter the api project*templateUseDemoForDb.Api*and modify the database connection in the configuration file*appsettings.json*(same as just now)
Then run the api project
```
> dotnet restore
> dotnet run
```
Wait for a while and get the following result indicating success
```
[16:50:07 INF] Now listening on: http://localhost:5000
[16:50:07 INF] Now listening on: https://localhost:5001
[16:50:07 INF] Application started. Press Ctrl + C to shut down.
[16:50:07 INF] Hosting environment: Production
[16:50:07 INF] Content root path: xxxxx\templateusedemofordb.api
```

## Test interface
Send using postman or directly using the browser
```
# GET
http://localhost:5000/product?pageSize=2&pageIndex=0&orderBy=fullname&fields=id,name,description
```
Get similar json return table success
``` json
{
    "statusCode": 200,
    "message": "Request successful.",
    "isError": false,
    "result": [
        {
            "id": "16735bcb-03ba-4468-ac0d-a0792d7299ad",
            "name": "A Learning ASP.NET Core",
            "description": "C best-selling book covering the fundamentals of ASP.NET Core"
        },
        {
            "id": "7c48a0f2-b790-4db2-bd68-8e62a03ae09e",
            "name": "C Learning .NET Core",
            "description": "D best-selling book covering the fundamentals of .NET Core"
        }
    ]
}
```

## This template project follows the agreement
[Apache License 2.0](https://github.com/larssonsun/RESTfulAPITemplate/blob/master/LICENSE) license.

Copyright (c) 2020-present Larssonsun