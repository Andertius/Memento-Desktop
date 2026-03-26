using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Memento.Avalonia.Data;

namespace Memento.Avalonia.ViewModels;

public partial class LearnViewModel() : PageViewModel(ApplicationPageNames.Learn)
{
    [ObservableProperty]
    private string _test = "Learn view";

    public override async Task OnPageSelected()
    {
        await Task.Delay(10000);
    }
}
