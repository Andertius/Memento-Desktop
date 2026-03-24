using CommunityToolkit.Mvvm.ComponentModel;
using Memento.Avalonia.Data;

namespace Memento.Avalonia.ViewModels;

public partial class ManageTagsViewModel : PageViewModel
{
    [ObservableProperty]
    private string _test = "Manage tags page view";

    public ManageTagsViewModel()
    {
        PageName = ApplicationPageNames.ManageTags;
    }
}
