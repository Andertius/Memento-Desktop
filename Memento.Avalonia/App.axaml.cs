using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Memento.Avalonia.Extensions;
using Memento.Avalonia.ViewModels;
using Memento.Avalonia.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Memento.Avalonia;

public partial class App : Application
{
    private const string _environmentName = "Development";
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var configuration = BuildConfiguration();
        var serviceProvider = ConfigureServices(configuration);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();

            desktop.MainWindow = new MainView
            {
                DataContext = serviceProvider.GetRequiredService<MainViewModel>(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }

    private static IConfiguration BuildConfiguration()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{_environmentName}.json")
            .Build();

        return config;
    }

    private static ServiceProvider ConfigureServices(IConfiguration configuration)
    {
        var services = new ServiceCollection();
        services.AddViewModels();
        services.AddFactories();
        services.AddClients();
        services.AddServices();
        services.AddOptions(configuration);

        return services.BuildServiceProvider();
    }
}
