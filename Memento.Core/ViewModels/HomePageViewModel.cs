using Memento.Core.Data;
using Memento.Core.Interfaces.ViewModels;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels;

public partial class HomePageViewModel() : PageViewModel(ApplicationPageNames.HomePage), IHomePageViewModel
{
    [Reactive]
    private string _test = "Home page view";
}
