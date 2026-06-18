using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Memento.Avalonia.Views;
using Memento.Core.ViewModels;
using Splat;

namespace Memento.Avalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var viewModel = AppLocator.Current.GetService<MainViewModel>();
            desktop.MainWindow = new MainView { DataContext = viewModel };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
