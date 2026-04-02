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
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;

namespace Memento.Core.ViewModels.CardViewModels;

public partial class EditCardViewModel : DialogViewModelBase, IEditCardViewModel, IValidatableViewModel
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

        var wordValidation = this.WhenAnyValue(x => x.Card.Word, word => !String.IsNullOrWhiteSpace(word));
        var translationValidation = this.WhenAnyValue(x => x.Card.Translation, translation => !String.IsNullOrWhiteSpace(translation));

        var canSave = wordValidation.CombineLatest(translationValidation).Select(notEmpty => notEmpty is { First: true, Second: true });
        SaveCardCommand = ReactiveCommand.CreateFromTask(SaveCardAsync, canSave);

        var canDeleteImage = Card.WhenAnyValue<CardViewModel, bool, Uri?>(x => x.ImageUrl, uri => uri is not null);
        DeleteImageCommand = ReactiveCommand.Create(DeleteImage, canDeleteImage);

        this.ValidationRule(
            viewModel => viewModel.Card.Word,
            wordValidation,
            "Word cannot be empty");

        this.ValidationRule(
            viewModel => viewModel.Card.Translation,
            translationValidation,
            "Translation cannot be empty");
    }

    public IValidationContext ValidationContext { get; } = new ValidationContext();

    public Interaction<Unit, ImageData?> OpenFile { get; } = new();

    public ReactiveCommand<Unit, Unit> SaveCardCommand { get; }

    public ReactiveCommand<Unit, Unit> DeleteImageCommand { get; }

    public bool Deleted { get; private set; }

    public bool Canceled { get; private set; }

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

        Canceled = true;
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

    public void DeleteImage()
    {
        Card.UploadedImageData = null;
        Card.ImageUrl = null;
    }
}
