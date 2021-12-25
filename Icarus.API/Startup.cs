using AutoMapper;
using Hangfire;
using Hangfire.MemoryStorage;
using Icarus.API.Infrastructure;
using Icarus.API.Services;
using Icarus.Service.Product;
using Icarus.Service.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Icarus.API
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
            var _mappingProfile = new MapperConfiguration(mp => { mp.AddProfile(new MappingProfile()); });
            IMapper mapper = _mappingProfile.CreateMapper();

            // Mapper'ýn inject edildiði kýsým
            services.AddSingleton(mapper);

            // User ve Product interface'lerinin alakalý class'lara inject edildiði kýsým
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProductService, ProductService>();

            // In-Memory cache burada ekleniyordu
            //services.AddMemoryCache();

            // redis cache burada ekleniyor ve kullandýðýmýz server'ý giriyoruz
            // kullandýðýmýz server = localhost'taki 6379 portu
            services.AddStackExchangeRedisCache(options => options.Configuration = "localhost:6379");

            services.AddHangfire(configuration =>
                                configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                                .UseSimpleAssemblyNameTypeSerializer()
                                .UseDefaultTypeSerializer()
                                .UseMemoryStorage());

            services.AddHangfireServer();
            services.AddSingleton<IPrintWelcomeJob, PrintWelcomeJob>();

            services.AddScoped<LoginFilter>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Icarus.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
                              IBackgroundJobClient backgroundJobClient,
                              IRecurringJobManager recurringJobManager,
                              IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Icarus.API v1"));
            }

            app.UseHangfireDashboard();

            backgroundJobClient.Enqueue(() => serviceProvider.GetService<IPrintWelcomeJob>().PrintWelcome());

            recurringJobManager.AddOrUpdate(
                "Run every minute",
                () => new PrintWelcomeJob().PrintWelcome(),
                "* * * * *"
                );
            
            recurringJobManager.AddOrUpdate(
                 "Run every minute",
                 () => serviceProvider.GetService<IPrintWelcomeJob>().CleanUserTable(),
                 "* * * * *"
                 );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
