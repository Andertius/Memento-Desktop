using Memento.Core.Data;
using Memento.Core.Interfaces.ViewModels;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels;

public partial class SettingsViewModel() : PageViewModel(ApplicationPageNames.Settings), ISettingsViewModel
{
    [Reactive]
    private string _test = "Settings page view";
}
