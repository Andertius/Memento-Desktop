using Memento.Core.Data;
using Memento.Core.Interfaces.ViewModels;

namespace Memento.Avalonia.DesignTime.ViewModels;

public sealed class SettingsViewModel() : PageViewModel(ApplicationPageNames.Settings), ISettingsViewModel
{
    public string Test { get; set; } = "Settings view";
}
