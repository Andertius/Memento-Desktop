using CommunityToolkit.Mvvm.ComponentModel;
using Memento.Avalonia.Data;

namespace Memento.Avalonia.ViewModels;

public partial class HomePageViewModel : PageViewModel
{
    [ObservableProperty]
    private string _test = "Home page view";

    public HomePageViewModel()
    {
        PageName = ApplicationPageNames.HomePage;
    }
}
