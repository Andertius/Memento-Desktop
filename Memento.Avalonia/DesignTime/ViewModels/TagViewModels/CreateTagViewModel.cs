using System.Reactive;
using System.Threading.Tasks;
using Memento.Core.Interfaces.ViewModels.TagViewModels;
using Memento.Core.ViewModels.TagViewModels;
using ReactiveUI;

namespace Memento.Avalonia.DesignTime.ViewModels.TagViewModels;

public sealed class CreateTagViewModel : ViewModelBase, ICreateTagViewModel
{
    public TagViewModel Tag { get; set; } = new()
    {
        Name = "Name",
    };

    public ReactiveCommand<Unit, Unit> SaveTagCommand { get; } = ReactiveCommand.CreateFromTask(_ => Task.FromResult(Unit.Default));

    public ReactiveCommand<Unit, Unit> CancelCommand { get; } = ReactiveCommand.Create<Unit, Unit>(_ => Unit.Default);
}
