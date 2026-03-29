using Memento.Core.ViewModels;
using ReactiveUI.Avalonia;

namespace Memento.Avalonia.Views;

public partial class SettingsView : ReactiveUserControl<SettingsViewModel>
{
    public SettingsView()
    {
        InitializeComponent();
    }
}
