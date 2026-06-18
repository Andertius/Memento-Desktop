using System;
using Avalonia;
using Memento.Avalonia.Constants;
using Memento.Avalonia.Extensions;
using Memento.Avalonia.Helpers;
using Microsoft.Extensions.Configuration;
using ReactiveUI.Avalonia.Splat;
#if RELEASE
using System.IO;
#endif

namespace Memento.Avalonia;

public sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
        => BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .UseReactiveUIWithMicrosoftDependencyResolver(services =>
            {
                services.AddViews();
                services.AddViewModels();
                services.AddFactories();
                services.AddClients();
                services.AddServices();
                services.AddOptions(BuildConfiguration());
            })
            .WithInterFont()
            .LogToTrace();

    private static IConfiguration BuildConfiguration()
    {
        string settingsDirectory = ConfigDirectoryHelper.GetAppSettingsDirectory();

        var config = new ConfigurationBuilder()
            .SetBasePath(settingsDirectory)
            .AddJsonFile(ConfigNames.AppSettingsFile)
            .Build();

        return config;
    }
}
