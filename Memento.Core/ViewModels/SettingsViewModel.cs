using Memento.Core.Data;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels;

public partial class SettingsViewModel() : PageViewModel(ApplicationPageNames.Settings)
{
    [Reactive]
    private string _test = "Settings page view";
}
