using Memento.Core.Data;
using Memento.Core.Interfaces.ViewModels;

namespace Memento.Avalonia.DesignTime.ViewModels;

public sealed class HomePageViewModel() : PageViewModel(ApplicationPageNames.HomePage), IHomePageViewModel
{
    public string Test { get; set; } = "Home page view";
}
