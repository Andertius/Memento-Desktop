using CommunityToolkit.Mvvm.ComponentModel;
using Memento.Avalonia.Data;

namespace Memento.Avalonia.ViewModels;

public partial class ManageCategoriesViewModel() : PageViewModel(ApplicationPageNames.ManageCategories)
{
    [ObservableProperty]
    private string _test = "Manage categories page view";
}
