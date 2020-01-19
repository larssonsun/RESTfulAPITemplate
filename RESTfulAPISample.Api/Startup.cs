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
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

#if (DBINMEMORY)

            services.AddDbContext<MyContext>(opt =>
                opt.UseInMemoryDatabase("RESTfulAPISampleMemoryDb"));

#elif (MSSQL)

            services.AddDbContext<MyContext>(options => options.UseSqlServer(
               Configuration.GetConnectionString("RESTfulAPISampleDbConnStr")));

#endif
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

#if (ENABLERESPONSECACHE)

            services.AddResponseCaching();
            services.AddHttpCacheHeaders(
            (e) =>
            {
                e.MaxAge = 30;
            },
            (v) =>
            {
                v.MustRevalidate = true;
            });

#endif

#if (LOCALMEMORYCACHE)

            services.AddMemoryCache(options =>
            {
                options.SizeLimit = 10; // 份数
                options.CompactionPercentage = 0.2; // 超出份数后的压缩比例，从低优先级的缓存开始压缩。这里就是压缩2份
            });

#elif (DISTRIBUTEDCACHE)

            services.AddDistributedMemoryCache();

#endif

            services.AddControllers()
                .AddFluentValidation(
                    options => options.RegisterValidatorsFromAssemblyContaining<Startup>().RunDefaultMvcValidationAfterFluentValidationExecutes = false
                ); // dto validattion

            // larsson：对链式验证进行短路“and”操作
            FluentValidation.ValidatorOptions.CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure; // dto validattion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

#if(ENABLERESPONSECACHE)

            app.UseResponseCaching();

            app.UseHttpCacheHeaders();
            
#endif

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

