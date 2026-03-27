using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Memento.Avalonia.HttpClients;
using Memento.Avalonia.Options;
using Memento.Avalonia.Services;
using Microsoft.Extensions.Options;

namespace Memento.Avalonia.ViewModels.CardViewModels;

public partial class CreateCardViewModel : DialogViewModel
{
    private readonly CardClientOptions _options;

    [ObservableProperty]
    private CardViewModel _card = new();

    private readonly ICardHttpClient _client;
    private readonly IFilesService _filesService;

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
        _filesService = null!;
        _options = null!;
    }

    public CreateCardViewModel(
        ICardHttpClient client,
        IFilesService filesService,
        IOptions<CardClientOptions> options)
    {
        _client = client;
        _filesService = filesService;
        _options = options.Value;
    }

    [RelayCommand]
    public async Task SaveCard()
    {
        var card = Card.ToDataModel();
        int id = await _client.AddCard(card);

        if (Card.UploadedImage is not null)
        {
            using var stream = new MemoryStream();
            Card.UploadedImage.Save(stream);
            stream.Position = 0;

            await _client.UploadImage(id, stream);
            Card.ImageUrl = $"{_options.Host}/images/cards/{id}.png";
            Card.UploadedImage = null;
        }

        Card.Id = id;
        Close();
    }

    [RelayCommand]
    public void Cancel()
    {
        Card.UploadedImage = null;
        Close();
    }

    [RelayCommand]
    public async Task UploadImage()
    {
        Card.UploadedImage = await _filesService.GetBitmap();
    }

    [RelayCommand]
    public void DeleteImage()
    {
        Card.UploadedImage = null;
        Card.ImageUrl = null;
    }
}
