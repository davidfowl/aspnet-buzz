using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Runtime;

namespace AspNet.Buzz
{
    public class Startup
    {
        public Startup(IHostingEnvironment hostingEnv)
        {
            var configuration = new Configuration()
                    .AddJsonFile("config.json");

            if (hostingEnv.IsEnvironment("Development"))
            {
                configuration = configuration.AddUserSecrets();
            }

            configuration.AddEnvironmentVariables();

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();

            services.AddSingleton<GithubEventUpdater>();
            services.AddSingleton<GithubEventHandler>();
            services.AddSingleton<EventPublishing>();

            services.Configure<GithubOptions>(Configuration.GetSubKey("Github"));
        }

        public void Configure(IApplicationBuilder app, 
                              IHostingEnvironment hostingEnv,
                              ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            app.UseErrorPage();

            app.UseStaticFiles();
            app.UseSignalR();

            app.ApplicationServices.GetService<GithubEventUpdater>().Initialize();
            app.ApplicationServices.GetService<EventPublishing>();
        }
    }
}
