using CommunityToolkit.Mvvm.ComponentModel;

namespace Memento.Avalonia.ViewModels;

public partial class ManageTagsViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _test = "Manage tags page view";
}
