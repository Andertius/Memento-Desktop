using System.Threading.Tasks;
using Memento.Core.Data;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels;

public abstract partial class PageViewModel(ApplicationPageNames pageName) : ViewModelBase
{
    [Reactive]
    private ApplicationPageNames _pageName = pageName;

    public virtual Task OnPageSelected()
        => Task.CompletedTask;
}
