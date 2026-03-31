using System.Reactive;
using Memento.Core.Data;
using ReactiveUI;

namespace Memento.Core.Interfaces.ViewModels;

public interface IMainViewModel : IViewModelBase
{
    string Username { get; set; }

    IPageViewModel CurrentPage { get; set; }

    ReactiveCommand<ApplicationPageNames, Unit> GoToPageCommand { get; }
}
