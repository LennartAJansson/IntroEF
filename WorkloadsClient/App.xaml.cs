using System;
using System.Windows;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using WorkloadsClient.ViewModels;
using WorkloadsClient.Views;

namespace WorkloadsClient
{
    public partial class App : Application
    {
        private readonly IHost host;

        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => ConfigureServices(context, services))
                .Build();

            ServiceProvider = host.Services;
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            //By having this extension method we achieve loose coupling:
            //With configuration given from AddWorkloadDb we return configuration.GetConnectionString
            services.AddWorkloadDb(configuration => configuration.GetConnectionString("Workloads"), ServiceLifetime.Transient);

            services.AddSingleton<MainViewModel>();

            services.AddTransient<MainWindow>();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(context.Configuration)
                .CreateLogger();

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger));
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await host.StartAsync();
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (host)
            {
                await host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }
    }

}
