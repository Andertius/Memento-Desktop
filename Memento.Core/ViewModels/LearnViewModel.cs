using Memento.Core.Data;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels;

public partial class LearnViewModel() : PageViewModel(ApplicationPageNames.Learn)
{
    [Reactive]
    private string _test = "Learn view";
}
