using CommunityToolkit.Mvvm.ComponentModel;

namespace Memento.Avalonia.ViewModels;

public partial class LearnViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _test = "Learn view";
}
