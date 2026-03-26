using CommunityToolkit.Mvvm.ComponentModel;
using Memento.Avalonia.Data;

namespace Memento.Avalonia.ViewModels;

public partial class SettingsViewModel() : PageViewModel(ApplicationPageNames.Settings)
{
    [ObservableProperty]
    private string _test = "Settings page view";
}
