using System.Reactive;
using Memento.Core.ViewModels.TagViewModels;
using ReactiveUI;

namespace Memento.Core.Interfaces.ViewModels.TagViewModels;

public interface ICreateTagViewModel : IViewModelBase
{
    TagViewModel Tag { get; set; }

    ReactiveCommand<Unit, Unit> SaveTagCommand { get; }
    
    ReactiveCommand<Unit, Unit> CancelCommand { get; }
}
