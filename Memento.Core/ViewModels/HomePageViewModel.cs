using Memento.Core.Data;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels;

public partial class HomePageViewModel() : PageViewModel(ApplicationPageNames.HomePage)
{
    [Reactive]
    private string _test = "Home page view";
}
