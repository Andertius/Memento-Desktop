using CommunityToolkit.Mvvm.ComponentModel;

namespace Memento.Avalonia.ViewModels;

public partial class ManageCategoriesViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _test = "Manage categories page view";
}
