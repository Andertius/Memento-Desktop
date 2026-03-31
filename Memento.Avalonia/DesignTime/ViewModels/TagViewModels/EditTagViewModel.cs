using System.Threading.Tasks;
using System.Reactive;
using Memento.Avalonia.DesignTime.ViewModels.DialogViewModels;
using Memento.Core.Interfaces.ViewModels.TagViewModels;
using Memento.Core.ViewModels.DialogViewModels;
using Memento.Core.ViewModels.TagViewModels;
using ReactiveUI;

namespace Memento.Avalonia.DesignTime.ViewModels.TagViewModels;

public sealed class EditTagViewModel : ViewModelBase, IEditTagViewModel
{
    public TagViewModel Tag { get; set; } = new()
    {
        Name = "Name",
    };

    public DialogViewModelBase DialogViewModel { get; set; } = new EmptyDialogViewModel();

    public ReactiveCommand<Unit, Unit> SaveTagCommand { get; } = ReactiveCommand.CreateFromTask(_ => Task.FromResult(Unit.Default));

    public ReactiveCommand<Unit, Unit> CancelCommand { get; } = ReactiveCommand.Create<Unit, Unit>(_ => Unit.Default);

    public ReactiveCommand<Unit, Unit> DeleteTagCommand { get; } = ReactiveCommand.CreateFromTask(_ => Task.FromResult(Unit.Default));
}
