using Memento.Core.Data;
using Memento.Core.Interfaces.ViewModels;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels;

public partial class LearnViewModel() : PageViewModel(ApplicationPageNames.Learn), ILearnViewModel
{
    [Reactive]
    private string _test = "Learn view";
}
