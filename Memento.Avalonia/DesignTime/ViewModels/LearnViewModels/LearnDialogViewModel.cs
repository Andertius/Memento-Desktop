using System;
using System.Reactive;
using System.Threading.Tasks;
using Memento.Core.DataModels;
using Memento.Core.Interfaces.ViewModels.LearnViewModels;
using ReactiveUI;

namespace Memento.Avalonia.DesignTime.ViewModels.LearnViewModels;

public sealed class LearnDialogViewModel : ILearnDialogViewModel
{
    public Card CurrentCard { get; set; } = new()
    {
        Word = "Word",
        Translation = "Translation text",
        Definition = "Definition text",
        Hint = "Hint text",
    };

    public string? Hint { get; set; } = "Hint text";

    public bool FaceDown { get; set; }

    public Uri? ImageUrl { get; set; }

    public ReactiveCommand<Unit, Unit> MoveNextCommand { get; } = ReactiveCommand.CreateFromTask(_ => Task.FromResult(Unit.Default));

    public ReactiveCommand<Unit, Unit> ShowHintCommand { get; } = ReactiveCommand.CreateFromTask(_ => Task.FromResult(Unit.Default));
    
    public ReactiveCommand<Unit, Unit> FlipCardCommand { get; } = ReactiveCommand.CreateFromTask(_ => Task.FromResult(Unit.Default));
}
