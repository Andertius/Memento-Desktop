using Memento.Avalonia.Data;
using ReactiveUI.SourceGenerators;

namespace Memento.Avalonia.ViewModels;

public partial class SettingsViewModel() : PageViewModel(ApplicationPageNames.Settings)
{
    [Reactive]
    private string _test = "Settings page view";
}
