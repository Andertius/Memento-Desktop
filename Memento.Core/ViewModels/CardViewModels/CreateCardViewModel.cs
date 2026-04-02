using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Memento.Core.Data;
using Memento.Core.HttpClients;
using Memento.Core.Interfaces.ViewModels.CardViewModels;
using Memento.Core.Options;
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

public partial class CreateCardViewModel : DialogViewModelBase, ICreateCardViewModel, IValidatableViewModel
{
    private readonly ICardHttpClient _client;
    private readonly ApiClientOptions _options;

    [Reactive]
    private CardViewModel _card = new();

    [Reactive]
    private IReadOnlyCollection<CategoryViewModel> _availableCategories;

    [Reactive]
    private IReadOnlyCollection<TagViewModel> _availableTags;

    public CreateCardViewModel(
        ICardHttpClient client,
        IOptions<ApiClientOptions> options,
        IReadOnlyCollection<CategoryViewModel> categories,
        IReadOnlyCollection<TagViewModel> tags)
    {
        _client = client;
        _options = options.Value;
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

    public Interaction<Unit, ImageData> OpenFile { get; } = new();

    public ReactiveCommand<Unit, Unit> SaveCardCommand { get; }

    public ReactiveCommand<Unit, Unit> DeleteImageCommand { get; }

    public async Task SaveCardAsync()
    {
        var card = Card.ToDataModel();
        int id = await _client.AddCard(card);

        if (Card.UploadedImageData is not null)
        {
            string? fileName = await _client.UploadImage(id, Card.UploadedImageData);
            Card.ImageUrl = new Uri($"{_options.Host}/{ApiPaths.CardsImagesPath}/{fileName}");
            Card.UploadedImageData = null;
        }

        Card.Id = id;
        Close();
    }

    [ReactiveCommand]
    public void Cancel()
    {
        Card.UploadedImageData = null;
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
