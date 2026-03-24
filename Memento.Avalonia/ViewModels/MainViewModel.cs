using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Memento.Avalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private HomePageViewModel _homePageViewModel = new();

    [ObservableProperty]
    private LearnViewModel _learnViewModel = new();

    [ObservableProperty]
    private ManageCardsViewModel _manageCardsViewModel = new();

    [ObservableProperty]
    private ManageCategoriesViewModel _manageCategoriesViewModel = new();

    [ObservableProperty]
    private ManageTagsViewModel _manageTagsViewModel = new();

    [ObservableProperty]
    private SettingsViewModel _settingsViewModel = new();

    [ObservableProperty]
    private string _username = "Spaghet";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HomePageIsActive))]
    [NotifyPropertyChangedFor(nameof(LearnIsActive))]
    [NotifyPropertyChangedFor(nameof(ManageCardsIsActive))]
    [NotifyPropertyChangedFor(nameof(ManageCategoriesIsActive))]
    [NotifyPropertyChangedFor(nameof(ManageTagsIsActive))]
    [NotifyPropertyChangedFor(nameof(SettingsIsActive))]
    private ViewModelBase _currentPage;

    public bool HomePageIsActive => CurrentPage == HomePageViewModel;
    public bool LearnIsActive => CurrentPage == LearnViewModel;
    public bool ManageCardsIsActive => CurrentPage == ManageCardsViewModel;
    public bool ManageCategoriesIsActive => CurrentPage == ManageCategoriesViewModel;
    public bool ManageTagsIsActive => CurrentPage == ManageTagsViewModel;
    public bool SettingsIsActive => CurrentPage == SettingsViewModel;

    public MainViewModel()
    {
        CurrentPage = _homePageViewModel;
    }

    [RelayCommand]
    public void GoToPage(object? parameter)
    {
        if (parameter is not ViewModelBase viewModel)
        {
            return;
        }

        CurrentPage = viewModel;
    }
}
