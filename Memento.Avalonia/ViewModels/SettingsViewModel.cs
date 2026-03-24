using CommunityToolkit.Mvvm.ComponentModel;
using Memento.Avalonia.Data;

namespace Memento.Avalonia.ViewModels;

public partial class SettingsViewModel : PageViewModel
{
    [ObservableProperty]
    private string _test = "Settings page view";

    public SettingsViewModel()
    {
        PageName = ApplicationPageNames.Settings;
    }
}
