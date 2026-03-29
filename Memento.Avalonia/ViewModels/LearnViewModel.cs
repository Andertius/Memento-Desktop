using System.Threading.Tasks;
using Memento.Avalonia.Data;
using ReactiveUI.SourceGenerators;

namespace Memento.Avalonia.ViewModels;

public partial class LearnViewModel() : PageViewModel(ApplicationPageNames.Learn)
{
    [Reactive]
    private string _test = "Learn view";

    public override async Task OnPageSelected()
    {
        await Task.Delay(10000);
    }
}
