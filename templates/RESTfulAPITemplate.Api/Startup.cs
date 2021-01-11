using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RESTfulAPITemplate.Core.Configuration.PropertyMapping;
using RESTfulAPITemplate.Core.Interface;
using RESTfulAPITemplate.Infrastructure;
using RESTfulAPITemplate.Infrastructure.Repository;
using Serilog;
#if (RESTFULAPIHELPER) 
using Larsson.RESTfulAPIHelper;
using RESTfulAPITemplate.Core.Configuration.SortMapping;
#endif
#if (RESPONSEHANDLERWRAPPER)
using AutoWrapper;
using RESTfulAPITemplate.Middleware;
#endif
#if (ENABLESWAGGER)
using Microsoft.OpenApi.Models;
using System.IO;
using System;
#endif
#if (ENABLEJWTAUTHENTICATION)
using RESTfulAPITemplate.Api.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RESTfulAPITemplate.Core.DomainModel;
using System.Text;
#endif
#if (SCETIAAUTHENTICATION)
using RESTfulAPITemplate.Infrastructure.Util;
#endif

namespace RESTfulAPITemplate.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

#if (ENABLEJWTAUTHENTICATION)

            services.AddScoped<IAuthenticateService, TokenAuthenticationService>();

#if (SCETIAAUTHENTICATION)

            services.AddScoped<IUserRepository, ScetiaUserRepository>();

#else

            services.AddScoped<IUserRepository, UserRepository>();
            
#endif

#endif

            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

#if (DBINMEMORY)

            services.AddDbContext<DemoContext>(dcob =>
                dcob.UseInMemoryDatabase("RESTfulAPITemplateMemoryDb"));

#elif (MSSQL)

            services.AddDbContext<DemoContext>(dcob => 
            {

#if (ROWNUMBERINEF3)
                
                dcob.ReplaceService<Microsoft.EntityFrameworkCore.Query.IQueryTranslationPostprocessorFactory, Scetia.App.Server.Extention.SqlServer2008QueryTranslationPostprocessorFactory>();

#endif
                dcob.UseSqlServer(Configuration.GetConnectionString("RESTfulAPITemplateDbConnStr")

#if (OBSOLETESQLSERVER)

                , soa => soa.UseRowNumberForPaging()

#endif

            );
            });

#endif

#if (SCETIAAUTHENTICATION)

            services.AddDbContext<ScetiaIndentityContext>(dcob => dcob.UseSqlServer(Configuration.GetConnectionString("ScetiaIdentityDbConnStr")

#if (OBSOLETESQLSERVER)

                , soa => soa.UseRowNumberForPaging()

#endif

            ));

            services.AddScoped<IScetiaIndentityUtil, ScetiaIndentityUtil>();

#endif

#if (DBINMEMORY)

            services.AddScoped<DemoContextSeed>();

#endif

            var mappingConfig = new MapperConfiguration(ice =>
            {
                // autoMapper: Mapping null strings to string.Empty during mapping
                ice.ValueTransformers.Add<string>(tf => tf ?? string.Empty);

                ice.AddProfile(new PropertyMappings());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

#if (RESTFULAPIHELPER)

            services.AddRESTfulAPIHelper(rho => rho.Register<ProductSortMapping>());

#endif

            services.Configure<ApiBehaviorOptions>(abo =>
            {
                abo.SuppressModelStateInvalidFilter = true;
            });

#if (ENABLERESPONSECACHE)

            services.AddResponseCaching();
            services.AddHttpCacheHeaders(
            (e) => { e.MaxAge = 30; },
            (v) => { v.MustRevalidate = true; });

#endif

#if (LOCALMEMORYCACHE)

            services.AddMemoryCache(mco =>
            {
                mco.SizeLimit = 10; // 份数
                mco.CompactionPercentage = 0.2; // 超出份数后的压缩比例，从低优先级的缓存开始压缩。这里就是压缩2份
            });

#elif (DISTRIBUTEDCACHE)

            services.AddDistributedMemoryCache();

#endif

            // larsson：
            // ReturnHttpNotAcceptable属性设为true，如果想要的格式不支持，那么就会返回406 Not Acceptable
            // 比如请求头中Acccept Header是application/xml，而响应头content-type返回的是application/json时会返回406
            // 如果不指定Accept Header的情况下就返回默认的json格式
            services.AddControllers(mo => mo.ReturnHttpNotAcceptable = true)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss.fff";
                    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                }) // json return from controll action will be format with camel type.
                .AddFluentValidation(
                    fvmc => fvmc.RegisterValidatorsFromAssemblyContaining<Core.Interface.IUnitOfWork>().RunDefaultMvcValidationAfterFluentValidationExecutes = false
                ); // dto validattion

            // larsson：对链式验证进行短路“and”操作
            FluentValidation.ValidatorOptions.CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure; // dto validattion

#if (ENABLEJWTAUTHENTICATION)

            services.Configure<TokenManagement>(Configuration.GetSection("tokenManagement"));
            var token = Configuration.GetSection("tokenManagement").Get<TokenManagement>();
            services.AddAuthentication(ao =>
            {
                ao.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                ao.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jbo =>
            {
                jbo.RequireHttpsMetadata = false;
                jbo.SaveToken = true;
                jbo.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                    ValidIssuer = token.Issuer,
                    ValidAudience = token.Audience,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

#endif

#if (ENABLESWAGGER)

            services.AddSwaggerGen(sgo =>
            {
                sgo.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "RESTfulAPITemplate API",
                    Version = "v1",
                    Description = "API for RESTfulAPITemplate",
                    Contact = new OpenApiContact { Name = "Larsson", Email = "77540975@qq.com", Url = new Uri("https://blog.larssonsun.net") }
                });

                // include document file
                sgo.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(Startup).Assembly.GetName().Name}.xml"), true);

                // add security definitions
                sgo.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
                sgo.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference()
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    }, Array.Empty<string>() }
                });
            });

#endif

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseSerilogRequestLogging();

#if (ENABLERESPONSECACHE)

            app.UseResponseCaching();

            app.UseHttpCacheHeaders();

#endif

#if (RESPONSEHANDLERWRAPPER)

            app.UseFixAutoWrapperMiddleware(fasa =>
            {
                fasa.HttpStatusForce200 = false;
                fasa.SwaggerStartsWithSegments = "/" + Configuration["Publics:SwaggerStartsWithSegments"];
            });

            app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions
            {
                ShowStatusCode = true,
                UseApiProblemDetailsException = true,
                ShowIsErrorFlagForSuccessfulResponse = true
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

#endif

#if (ENABLESWAGGER)

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(suo =>
            {
                suo.SwaggerEndpoint("/swagger/v1/swagger.json", "RESTfulAPITemplate API V1");
                suo.RoutePrefix = Configuration["Publics:SwaggerStartsWithSegments"]; ;
                suo.DocumentTitle = "RESTfulAPITemplate API";
            });

#endif

            app.UseRouting();

#if (ENABLEJWTAUTHENTICATION)

            app.UseAuthentication();

#endif

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

