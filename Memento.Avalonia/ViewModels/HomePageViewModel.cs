using Memento.Avalonia.Data;
using ReactiveUI.SourceGenerators;

namespace Memento.Avalonia.ViewModels;

public partial class HomePageViewModel() : PageViewModel(ApplicationPageNames.HomePage)
{
    [Reactive]
    private string _test = "Home page view";
}
