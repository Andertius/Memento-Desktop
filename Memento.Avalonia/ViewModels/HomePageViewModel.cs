using CommunityToolkit.Mvvm.ComponentModel;
using Memento.Avalonia.Data;

namespace Memento.Avalonia.ViewModels;

public partial class HomePageViewModel() : PageViewModel(ApplicationPageNames.HomePage)
{
    [ObservableProperty]
    private string _test = "Home page view";
}
