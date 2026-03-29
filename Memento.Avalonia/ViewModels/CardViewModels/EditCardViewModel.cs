using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Memento.Avalonia.Data;
using Memento.Avalonia.HttpClients;
using Memento.Avalonia.Interfaces;
using Memento.Avalonia.Options;
using Memento.Avalonia.Services;
using Memento.Avalonia.ViewModels.DialogViewModels;
using Microsoft.Extensions.Options;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Memento.Avalonia.ViewModels.CardViewModels;

public partial class EditCardViewModel : DialogViewModelBase, IDialogProvider
{
    private readonly ApiClientOptions _options;
    private readonly ICardHttpClient _client;
    private readonly IDialogService _dialogService;
    private readonly string? _temporaryImageUrl;

    [Reactive]
    private CardViewModel _card;

    [Reactive]
    private DialogViewModelBase? _dialogViewModel;

    /// <summary>
    /// Design-time only constructor
    /// </summary>
    public EditCardViewModel()
    {
        if (!Design.IsDesignMode)
        {
            throw new InvalidOperationException("Default constructor only allowed in design-time");
        }

        _client = null!;
        _dialogService = null!;
        _options = null!;
        _card = new CardViewModel();
        _temporaryImageUrl = Card.ImageUrl;
    }

    public EditCardViewModel(
        ICardHttpClient client,
        IDialogService dialogService,
        IOptions<ApiClientOptions> options,
        CardViewModel card)
    {
        _client = client;
        _dialogService = dialogService;
        _options = options.Value;
        _card = card;
        _temporaryImageUrl = Card.ImageUrl;
    }

    public Interaction<Unit, ImageData> OpenFile { get; } = new();

    public bool Deleted { get; private set; }

    [ReactiveCommand]
    public async Task SaveCardAsync()
    {
        if (Card.UploadedImage is not null && !String.IsNullOrWhiteSpace(Card.UploadedImageName))
        {
            using var stream = new MemoryStream();
            Card.UploadedImage.Save(stream);
            stream.Position = 0;

            string? fileName = await _client.UploadImage(Card.Id, Card.UploadedImageName, stream);
            Card.ImageUrl = $"{_options.Host}/{ApiPaths.CardsImagesPath}/{fileName}";
            Card.UploadedImage = null;
        }
        else
        {
            await _client.DeleteImage(Card.Id);
        }

        await _client.UpdateCard(Card.ToDataModel());
        Close();
    }

    [ReactiveCommand]
    public void Cancel()
    {
        Card.ImageUrl = _temporaryImageUrl;
        Card.UploadedImage = null;
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
        (Card.UploadedImage, Card.UploadedImageName) = await OpenFile.Handle(Unit.Default);
    }

    [ReactiveCommand]
    public void DeleteImage()
    {
        Card.UploadedImage = null;
        Card.ImageUrl = null;
    }
}
