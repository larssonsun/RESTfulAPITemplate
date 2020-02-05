using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RESTfulAPISample.Api.Configurations;
using RESTfulAPISample.Core.Interface;
using RESTfulAPISample.Infrastructure;
using RESTfulAPISample.Infrastructure.Repository;
using AutoWrapper;
#if (ENABLESWAGGER)
using Microsoft.OpenApi.Models;
using System.IO;
using System;
#endif
#if (ENABLEJWTAUTHENTICATION)
using RESTfulAPISample.Api.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RESTfulAPISample.Core.DomainModel;
using System.Text;
using RESTfulAPISample.Middleware;
#endif
namespace RESTfulAPISample.Api
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

            services.AddScoped<IUserRepository, UserRepository>();

#endif

            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

#if (DBINMEMORY)

            services.AddDbContext<MyContext>(dcob =>
                dcob.UseInMemoryDatabase("RESTfulAPISampleMemoryDb"));

#elif (MSSQL)

            services.AddDbContext<MyContext>(dcob => dcob.UseSqlServer(
               Configuration.GetConnectionString("RESTfulAPISampleDbConnStr")));

#endif
            var mappingConfig = new MapperConfiguration(ice =>
            {
                ice.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

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

            services.AddControllers()
                .AddFluentValidation(
                    fvmc => fvmc.RegisterValidatorsFromAssemblyContaining<Startup>().RunDefaultMvcValidationAfterFluentValidationExecutes = false
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
                    Title = "RESTfulAPISample API",
                    Version = "v1",
                    Description = "API for RESTfulAPISample",
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
            app.UseCatchTheLastMiddleware();

            app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions { ShowStatusCode = true });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

#if (ENABLESWAGGER)

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(suo =>
            {
                suo.SwaggerEndpoint("/swagger/v1/swagger.json", "RESTfulAPISample API V1");
                suo.RoutePrefix = "swagger"; //larsson：为了AutoWrapper能正常识别swagger这里必须要设置一个前缀而不是string.Empty
                suo.DocumentTitle = "RESTfulAPISample API";
            });

#endif

            app.UseRouting();

#if (ENABLERESPONSECACHE)

            app.UseResponseCaching();

            app.UseHttpCacheHeaders();

#endif

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

