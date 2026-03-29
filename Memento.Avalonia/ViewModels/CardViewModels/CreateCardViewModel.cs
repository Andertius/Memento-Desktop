using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Memento.Avalonia.Data;
using Memento.Avalonia.HttpClients;
using Memento.Avalonia.Options;
using Memento.Avalonia.ViewModels.DialogViewModels;
using Microsoft.Extensions.Options;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Memento.Avalonia.ViewModels.CardViewModels;

public partial class CreateCardViewModel : DialogViewModelBase
{
    private readonly ApiClientOptions _options;

    [Reactive]
    private CardViewModel _card = new();

    private readonly ICardHttpClient _client;

    /// <summary>
    /// Design-time only constructor
    /// </summary>
    public CreateCardViewModel()
    {
        if (!Design.IsDesignMode)
        {
            throw new InvalidOperationException("Default constructor only allowed in design-time");
        }

        _client = null!;
        _options = null!;
    }

    public CreateCardViewModel(
        ICardHttpClient client,
        IOptions<ApiClientOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public Interaction<Unit, ImageData> OpenFile { get; } = new();

    [ReactiveCommand]
    public async Task SaveCardAsync()
    {
        var card = Card.ToDataModel();
        int id = await _client.AddCard(card);

        if (Card.UploadedImage is not null && !String.IsNullOrWhiteSpace(Card.UploadedImageName))
        {
            using var stream = new MemoryStream();
            Card.UploadedImage.Save(stream);
            stream.Position = 0;

            string? fileName = await _client.UploadImage(id, Card.UploadedImageName, stream);
            Card.ImageUrl = $"{_options.Host}/{ApiPaths.CardsImagesPath}/{fileName}";
            Card.UploadedImage = null;
        }

        Card.Id = id;
        Close();
    }

    [ReactiveCommand]
    public void Cancel()
    {
        Card.UploadedImage = null;
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
