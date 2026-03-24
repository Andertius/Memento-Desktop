using CommunityToolkit.Mvvm.ComponentModel;

namespace Memento.Avalonia.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _test = "Settings page view";
}
