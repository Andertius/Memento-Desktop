using System.Threading.Tasks;
using Memento.Avalonia.Data;
using ReactiveUI.SourceGenerators;

namespace Memento.Avalonia.ViewModels;

public abstract partial class PageViewModel(ApplicationPageNames pageName) : ViewModelBase
{
    [Reactive]
    private ApplicationPageNames _pageName = pageName;

    public virtual Task OnPageSelected()
        => Task.CompletedTask;
}
