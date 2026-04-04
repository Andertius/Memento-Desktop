using System;
using System.Collections.Generic;
using Memento.Core.DataModels;
using Memento.Core.Interfaces.ViewModels.LearnViewModels;
using Memento.Core.Options;
using Memento.Core.Services;
using Memento.Core.ViewModels.DialogViewModels;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels.LearnViewModels;

public partial class LearnDialogViewModel : DialogViewModelBase, ILearnDialogViewModel
{
    private readonly IReadOnlyList<Card> _cards;

    private int _currentIndex;

    [Reactive]
    private Card _currentCard;

    [Reactive]
    private string? _hint;

    [Reactive]
    private Uri? _imageUrl;

    [Reactive]
    private bool _faceDown;

    public LearnDialogViewModel(IReadOnlyList<Card> cards, ApiClientOptions options)
    {
        if (cards.Count == 0)
        {
            throw new InvalidOperationException("Can't learn 0 cards, idiot");
        }

        _cards = cards;
        _currentCard = cards[0];

        this.WhenAnyValue(x => x.CurrentCard)
            .Subscribe(card => ImageUrl = ImageHelper.GenerateCardImageUrl(card.Image, options.Host));
    }

    [ReactiveCommand]
    public void MoveNext()
    {
        _currentIndex++;
        FaceDown = false;

        if (_currentIndex == _cards.Count)
        {
            Close();

            return;
        }

        Hint = null;
        CurrentCard = _cards[_currentIndex];
    }

    [ReactiveCommand]
    public void ShowHint()
    {
        Hint = String.IsNullOrWhiteSpace(CurrentCard.Hint)
            ? "This card does not have a hint."
            : CurrentCard.Hint;
    }

    [ReactiveCommand]
    public void FlipCard()
        => FaceDown = true;
}
