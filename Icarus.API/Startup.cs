using AutoMapper;
using Hangfire;
using Hangfire.MemoryStorage;
using Icarus.API.Infrastructure;
using Icarus.API.Infrastructure.DistCache;
using Icarus.API.Jobs;
using Icarus.Service.Product;
using Icarus.Service.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

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

            // Mapper'�n inject edildi�i k�s�m
            services.AddSingleton(mapper);

            // User ve Product interface'lerinin alakal� class'lara inject edildi�i k�s�m
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProductService, ProductService>();

            // distrubed cache'i kullanca��m�z s�n�f� inject ediyoruz
            services.AddTransient<IDistCache, DistCache>();

            // In-Memory cache burada ekleniyordu
            //services.AddMemoryCache();

            // redis cache burada ekleniyor ve kulland���m�z server'� giriyoruz
            // kulland���m�z server = localhost'taki 6379 portu
            services.AddStackExchangeRedisCache(options => options.Configuration = "localhost:6379");

            services.AddHangfire(config =>
                        config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseDefaultTypeSerializer()
                        .UseMemoryStorage());

            services.AddHangfireServer();

            services.AddSingleton<IPrintWelcomeJob, PrintWelcomeJob>();

            // Custom Filterlerin inject edildi�i yer
            services.AddScoped<LoginFilter>();
            services.AddScoped<AuthFilter>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Icarus.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
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

            // IPrintWelcomeJob i�erisindeki 3 farkl� job �al��t�r�l�yor
            recurringJobManager.AddOrUpdate
                                ("Only recurring job",
                                 () => serviceProvider.GetService<IPrintWelcomeJob>().PrintWelcome(),
                                 Cron.Daily);
            recurringJobManager.AddOrUpdate
                                ("Only recurring job",
                                 () => serviceProvider.GetService<IPrintWelcomeJob>().CleanUserTable(),
                                 Cron.Hourly);
            recurringJobManager.AddOrUpdate
                                ("Only recurring job",
                                 () => serviceProvider.GetService<IPrintWelcomeJob>().CleanProductTable(),
                                 Cron.Hourly);

            app.UseCors(c => c.WithOrigins("https://localhost:5003").AllowAnyHeader().AllowAnyMethod());

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
