using Memento.Avalonia.ViewModels;
using ReactiveUI.Avalonia;

namespace Memento.Avalonia.Views;

public partial class SettingsView : ReactiveUserControl<SettingsViewModel>
{
    public SettingsView()
    {
        InitializeComponent();
    }
}
