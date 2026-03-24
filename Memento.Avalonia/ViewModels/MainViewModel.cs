using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Memento.Avalonia.Data;
using Memento.Avalonia.Factories;

namespace Memento.Avalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IPageFactory _pageFactory;
    
    [ObservableProperty]
    private string _username = "Spaghet";

    [ObservableProperty]
    private PageViewModel _currentPage = null!;

    /// <summary>
    /// Design-time constructor only.
    /// </summary>
    public MainViewModel()
    {
        _pageFactory = null!;
        _currentPage = new HomePageViewModel();
    }

    public MainViewModel(IPageFactory pageFactory)
    {
        _pageFactory = pageFactory;
        GoToPage(ApplicationPageNames.HomePage);
    }
    
    [RelayCommand]
    public void GoToPage(object? parameter)
    {
        if (parameter is not ApplicationPageNames pageName)
        {
            return;
        }

        CurrentPage = _pageFactory.GetPageViewModel(pageName);
    }
}
