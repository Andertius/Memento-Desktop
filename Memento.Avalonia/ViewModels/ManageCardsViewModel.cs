using CommunityToolkit.Mvvm.ComponentModel;

namespace Memento.Avalonia.ViewModels;

public partial class ManageCardsViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _test = "Manage cards page view";
}
