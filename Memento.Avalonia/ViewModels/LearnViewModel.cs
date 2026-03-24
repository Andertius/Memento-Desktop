using CommunityToolkit.Mvvm.ComponentModel;
using Memento.Avalonia.Data;

namespace Memento.Avalonia.ViewModels;

public partial class LearnViewModel : PageViewModel
{
    [ObservableProperty]
    private string _test = "Learn view";

    public LearnViewModel()
    {
        PageName = ApplicationPageNames.Learn;
    }
}
