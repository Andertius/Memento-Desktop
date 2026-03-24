using CommunityToolkit.Mvvm.ComponentModel;
using Memento.Avalonia.Data;

namespace Memento.Avalonia.ViewModels;

public partial class ManageCardsViewModel : PageViewModel
{
    [ObservableProperty]
    private string _test = "Manage cards page view";

    public ManageCardsViewModel()
    {
        PageName = ApplicationPageNames.ManageCards;
    }
}
