using System.Threading.Tasks;
using Memento.Core.Data;
using Memento.Core.Interfaces.ViewModels;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels;

public abstract partial class PageViewModel(ApplicationPageNames pageName) : ViewModelBase, IPageViewModel
{
    [Reactive]
    private ApplicationPageNames _pageName = pageName;

    public virtual Task OnPageSelected()
        => Task.CompletedTask;
}
