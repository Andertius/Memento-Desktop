using System.Collections.ObjectModel;
using System.Reactive;
using Memento.Core.ViewModels.TagViewModels;
using ReactiveUI;

namespace Memento.Core.Interfaces.ViewModels.TagViewModels;

public interface IManageTagsViewModel : IViewModelBase, IDialogProvider
{
    ObservableCollection<TagViewModel> Tags { get; set; }

    ReactiveCommand<Unit, Unit> CreateTagCommand { get; }

    ReactiveCommand<TagViewModel, Unit> EditTagCommand { get; }
}
