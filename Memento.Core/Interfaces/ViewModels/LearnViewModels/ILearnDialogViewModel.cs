using System;
using System.Reactive;
using Memento.Core.DataModels;
using ReactiveUI;

namespace Memento.Core.Interfaces.ViewModels.LearnViewModels;

public interface ILearnDialogViewModel
{
    Card CurrentCard { get; set; }

    Uri? ImageUrl { get; set; }

    string? Hint { get; set; }

    bool FaceDown { get; set; }

    ReactiveCommand<Unit, Unit> MoveNextCommand { get; }

    ReactiveCommand<Unit, Unit> ShowHintCommand { get; }

    ReactiveCommand<Unit, Unit> FlipCardCommand { get; }
}
