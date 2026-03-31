using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Memento.Core.Data;
using Memento.Core.HttpClients;
using Memento.Core.Interfaces.ViewModels.CardViewModels;
using Memento.Core.Options;
using Memento.Core.Services;
using Memento.Core.ViewModels.CategoryViewModels;
using Memento.Core.ViewModels.DialogViewModels;
using Memento.Core.ViewModels.TagViewModels;
using Microsoft.Extensions.Options;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels.CardViewModels;

public partial class EditCardViewModel : DialogViewModelBase, IEditCardViewModel
{
    private readonly ApiClientOptions _options;
    private readonly ICardHttpClient _client;
    private readonly IDialogService _dialogService;
    private readonly Uri? _temporaryImageUrl;

    [Reactive]
    private CardViewModel _card;

    [Reactive]
    private DialogViewModelBase? _dialogViewModel;

    [Reactive]
    private IReadOnlyCollection<CategoryViewModel> _availableCategories;

    [Reactive]
    private IReadOnlyCollection<TagViewModel> _availableTags;
    
    public EditCardViewModel(
        ICardHttpClient client,
        IDialogService dialogService,
        IOptions<ApiClientOptions> options,
        CardViewModel card,
        IReadOnlyCollection<CategoryViewModel> categories,
        IReadOnlyCollection<TagViewModel> tags)
    {
        _client = client;
        _dialogService = dialogService;
        _options = options.Value;
        _card = card;
        _temporaryImageUrl = Card.ImageUrl;
        _availableCategories = categories;
        _availableTags = tags;
    }

    public Interaction<Unit, ImageData?> OpenFile { get; } = new();

    public bool Deleted { get; private set; }

    [ReactiveCommand]
    public async Task SaveCardAsync()
    {
        if (Card.UploadedImageData is not null)
        {
            string? fileName = await _client.UploadImage(Card.Id, Card.UploadedImageData);
            Card.ImageUrl = new Uri($"{_options.Host}/{ApiPaths.CardsImagesPath}/{fileName}");
            Card.UploadedImageData = null;
        }
        else if (Card.ImageUrl is null)
        {
            await _client.DeleteImage(Card.Id);
        }

        await _client.UpdateCard(Card.ToDataModel());
        await _client.UpdateCardCategories(Card.Id, Card.Categories.Select(x => x.Id).ToArray());
        await _client.UpdateCardTags(Card.Id, Card.Tags.Select(x => x.Id).ToArray());
        Close();
    }

    [ReactiveCommand]
    public void Cancel()
    {
        Card.ImageUrl = _temporaryImageUrl;
        Card.UploadedImageData = null;
        Close();
    }

    [ReactiveCommand]
    public async Task DeleteCardAsync()
    {
        var confirmViewModel = new DeleteConfirmationDialogViewModel
        {
            DeletedObjectName = Card.Word,
        };

        await _dialogService.ShowDialogAsync(this, confirmViewModel);

        if (!confirmViewModel.Confirmed)
        {
            return;
        }

        await _client.DeleteCard(Card.Id);

        Deleted = true;
        Close();
    }

    [ReactiveCommand]
    public async Task UploadImageAsync()
    {
        Card.UploadedImageData = await OpenFile.Handle(Unit.Default);

        if (Card.UploadedImageData is { FilePath: not null })
        {
            Card.ImageUrl = Card.UploadedImageData.FilePath;
        }
    }

    [ReactiveCommand]
    public void DeleteImage()
    {
        Card.UploadedImageData = null;
        Card.ImageUrl = null;
    }
}
