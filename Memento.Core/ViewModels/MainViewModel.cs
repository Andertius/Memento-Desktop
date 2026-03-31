using System.Threading.Tasks;
using Memento.Core.Data;
using Memento.Core.Factories;
using Memento.Core.Interfaces.ViewModels;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels;

public partial class MainViewModel : ViewModelBase, IMainViewModel
{
    private readonly IPageViewModelFactory _pageViewModelFactory;

    [Reactive]
    private string _username = "Spaghet";

    [Reactive]
    private IPageViewModel _currentPage;

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
