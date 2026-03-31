using System.Threading.Tasks;
using Memento.Core.Data;
using Memento.Core.Interfaces.ViewModels;

namespace Memento.Avalonia.DesignTime.ViewModels;

public class PageViewModel(ApplicationPageNames pageName) : ViewModelBase, IPageViewModel
{
    public ApplicationPageNames PageName { get; set; } = pageName;

    public virtual Task OnPageSelected()
        => Task.CompletedTask;
}
