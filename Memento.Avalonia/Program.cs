using System;
using Avalonia;
using Memento.Avalonia.Extensions;
using Microsoft.Extensions.Configuration;
using ReactiveUI.Avalonia;
using ReactiveUI.Avalonia.Splat;

namespace Memento.Avalonia;

public sealed class Program
{
    private const string _environmentName = "Development";

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .RegisterReactiveUIViewsFromEntryAssembly()
            .UseReactiveUIWithMicrosoftDependencyResolver(
                services =>
                {
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
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{_environmentName}.json")
            .Build();

        return config;
    }
}
