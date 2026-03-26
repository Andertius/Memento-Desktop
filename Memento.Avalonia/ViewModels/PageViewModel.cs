using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Memento.Avalonia.Data;

namespace Memento.Avalonia.ViewModels;

public abstract partial class PageViewModel(ApplicationPageNames pageName) : ViewModelBase
{
    [ObservableProperty]
    private ApplicationPageNames _pageName = pageName;

    public virtual Task OnPageSelected()
        => Task.CompletedTask;
}
