using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Memento.Avalonia.Data;
using Memento.Avalonia.Factories;
using ReactiveUI.SourceGenerators;

namespace Memento.Avalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IPageViewModelFactory _pageViewModelFactory;

    [Reactive]
    private string _username = "Spaghet";

    [Reactive]
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
        CurrentPage = new HomePageViewModel();
    }

    public MainViewModel(IPageViewModelFactory pageViewModelFactory)
    {
        _pageViewModelFactory = pageViewModelFactory;
        CurrentPage = _pageViewModelFactory.GetPageViewModel(ApplicationPageNames.HomePage);
    }

    [ReactiveCommand]
    public async Task GoToPage(ApplicationPageNames pageName)
    {
        CurrentPage = _pageViewModelFactory.GetPageViewModel(pageName);
        await CurrentPage.OnPageSelected();
    }
}
