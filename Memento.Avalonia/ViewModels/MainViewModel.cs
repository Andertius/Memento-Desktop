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
    [NotifyPropertyChangedFor(nameof(HomePageIsActive))]
    [NotifyPropertyChangedFor(nameof(LearnIsActive))]
    [NotifyPropertyChangedFor(nameof(ManageCardsIsActive))]
    [NotifyPropertyChangedFor(nameof(ManageCategoriesIsActive))]
    [NotifyPropertyChangedFor(nameof(ManageTagsIsActive))]
    [NotifyPropertyChangedFor(nameof(SettingsIsActive))]
    private PageViewModel _currentPage;

    public bool HomePageIsActive => CurrentPage.PageName == ApplicationPageNames.HomePage;
    public bool LearnIsActive => CurrentPage.PageName == ApplicationPageNames.Learn;
    public bool ManageCardsIsActive => CurrentPage.PageName == ApplicationPageNames.ManageCards;
    public bool ManageCategoriesIsActive => CurrentPage.PageName == ApplicationPageNames.ManageCategories;
    public bool ManageTagsIsActive => CurrentPage.PageName == ApplicationPageNames.ManageTags;
    public bool SettingsIsActive => CurrentPage.PageName == ApplicationPageNames.Settings;

    public MainViewModel(IPageFactory pageFactory)
    {
        _pageFactory = pageFactory;
        _currentPage = _pageFactory.GetPageViewModel(ApplicationPageNames.HomePage);
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
