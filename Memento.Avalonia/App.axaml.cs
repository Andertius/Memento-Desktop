using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Memento.Avalonia.Data;
using Memento.Avalonia.Factories;
using Memento.Avalonia.Views;
using Memento.Avalonia.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Memento.Avalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        services.AddSingleton<MainViewModel>();
        services.AddTransient<HomePageViewModel>();
        services.AddTransient<SettingsViewModel>();
        services.AddTransient<LearnViewModel>();
        services.AddTransient<ManageCardsViewModel>();
        services.AddTransient<ManageCategoriesViewModel>();
        services.AddTransient<ManageTagsViewModel>();

        services.AddTransient<IPageFactory, PageFactory>();

        services.AddSingleton<Func<ApplicationPageNames, PageViewModel>>(sp => name => name switch
        {
            ApplicationPageNames.HomePage => sp.GetRequiredService<HomePageViewModel>(),
            ApplicationPageNames.Learn => sp.GetRequiredService<LearnViewModel>(),
            ApplicationPageNames.Settings => sp.GetRequiredService<SettingsViewModel>(),
            ApplicationPageNames.ManageCards => sp.GetRequiredService<ManageCardsViewModel>(),
            ApplicationPageNames.ManageCategories => sp.GetRequiredService<ManageCategoriesViewModel>(),
            ApplicationPageNames.ManageTags => sp.GetRequiredService<ManageTagsViewModel>(),
            _ => throw new InvalidOperationException("Provided page does not exist."),
        });

        var serviceProvider = services.BuildServiceProvider();

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

    private void DisableAvaloniaDataAnnotationValidation()
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
}
