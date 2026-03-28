using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Memento.Avalonia.Data;
using Memento.Avalonia.Factories;

namespace Memento.Avalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IPageViewModelFactory _pageViewModelFactory;

    [ObservableProperty]
    private string _username = "Spaghet";

    [ObservableProperty]
    private PageViewModel _currentPage;

    /// <summary>
    /// Design-time constructor only.
    /// </summary>
    public MainViewModel()
    {
        if (!Design.IsDesignMode)
        {
            throw new InvalidOperationException("Default constructor only allowed in design-time");
        }

        _pageViewModelFactory = null!;
        _currentPage = new HomePageViewModel();
    }

    public MainViewModel(IPageViewModelFactory pageViewModelFactory)
    {
        _pageViewModelFactory = pageViewModelFactory;
        _currentPage = _pageViewModelFactory.GetPageViewModel(ApplicationPageNames.HomePage);
    }

    [RelayCommand]
    public async Task GoToPage(ApplicationPageNames pageName)
    {
        CurrentPage = _pageViewModelFactory.GetPageViewModel(pageName);
        await CurrentPage.OnPageSelected();
    }
}
