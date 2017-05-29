using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using NetworkDictionary.Dispatcher.Interfaces;
using NetworkDictionary.Manager;
using NetworkDictionary.Manager.Interfaces;
using NetworkDictionary.Service.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace NetworkDictionary.Service
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<ManagerConfiguration>(Configuration.GetSection(nameof(ManagerConfiguration)));

            services.AddSingleton<IManager>(sp =>
            {
                var config = (IOptions<ManagerConfiguration>)sp.GetService(typeof(IOptions<ManagerConfiguration>));
                var options = new ManagerOptions(config.Value.ClearExpiredValuesPeriod, config.Value.DecreaseValueFrequincePeriod, config.Value.DefaultTtl, config.Value.MaxKeyCount);
                return ManagerFactory.CreateManager(options);
            });

            services.AddTransient<IDispatcher>(sp => {
                var manager = (IManager)sp.GetService(typeof(IManager));
                return new Dispatcher.Dispatcher(manager, true);
            });

            // Add swagger service
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Network dictionary API",
                    Description = "Network cached dictionary",
                    TermsOfService = "None",
                    Contact =
                        new Contact
                        {
                            Name = "Stepan Sychev",
                            Email = "stepan.sychev@gmail.com"
                        },
                    License = new License { Name = "(c) Stepan Sychev", Url = "" }
                });

                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "NetworkDictionary.Domain.xml");

                c.IncludeXmlComments(xmlPath);
            });

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Network dictionaty API v1"); });

            app.UseMvc();
        }
    }
}
