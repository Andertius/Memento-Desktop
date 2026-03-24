using CommunityToolkit.Mvvm.ComponentModel;

namespace Memento.Avalonia.ViewModels;

public partial class HomePageViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _test = "Home page view";
}
