using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Memento.Avalonia.DesignTime.ViewModels.DialogViewModels;
using Memento.Core.Interfaces.ViewModels.TagViewModels;
using Memento.Core.ViewModels.DialogViewModels;
using Memento.Core.ViewModels.TagViewModels;
using ReactiveUI;

namespace Memento.Avalonia.DesignTime.ViewModels.TagViewModels;

public sealed class ManageTagsViewModel : ViewModelBase, IManageTagsViewModel
{
    public DialogViewModelBase DialogViewModel { get; set; } = new EmptyDialogViewModel();

    public ObservableCollection<TagViewModel> Tags { get; set; } =
    [
        new() { Name = "Name 1" },
        new() { Name = "Name 2" },
    ];

    public ReactiveCommand<Unit, Unit> CreateTagCommand { get; } = ReactiveCommand.CreateFromTask(_ => Task.FromResult(Unit.Default));

    public ReactiveCommand<TagViewModel, Unit> EditTagCommand { get; } = ReactiveCommand.CreateFromTask<TagViewModel, Unit>(_ => Task.FromResult(Unit.Default));
}
