using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Memento.Core.Data;
using Memento.Core.HttpClients;
using Memento.Core.Options;
using Memento.Core.ViewModels.DialogViewModels;
using Microsoft.Extensions.Options;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels.CardViewModels;

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

    [ReactiveCommand]
    public void DeleteImage()
    {
        Card.UploadedImageData = null;
        Card.ImageUrl = null;
    }
}
