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

public partial class EditCardViewModel : DialogViewModel
{
    private readonly CardClientOptions _options;
    private readonly ICardHttpClient _client;
    private readonly IFilesService _filesSevice;

    [ObservableProperty]
    private CardViewModel _card;

    private string? temporaryImageUrl;

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
        _filesSevice = null!;
        _options = null!;
        _card = new CardViewModel();
        temporaryImageUrl = Card.ImageUrl;
    }

    public EditCardViewModel(
        ICardHttpClient client,
        IFilesService filesSevice,
        IOptions<CardClientOptions> options,
        CardViewModel card)
    {
        _client = client;
        _filesSevice = filesSevice;
        _options = options.Value;
        _card = card;
        temporaryImageUrl = Card.ImageUrl;
    }

    [RelayCommand]
    public async Task SaveCard()
    {
        if (Card.UploadedImage is not null)
        {
            using var stream = new MemoryStream();
            Card.UploadedImage.Save(stream);
            stream.Position = 0;

            await _client.UploadImage(Card.Id, stream);
            Card.ImageUrl = $"{_options.Host}/images/cards/{Card.Id}.png";
            Card.UploadedImage = null;
        }
        else
        {
            await _client.DeleteImage(Card.Id);
        }

        await _client.UpdateCard(Card.ToDataModel());
        Close();
    }

    [RelayCommand]
    public void Cancel()
    {
        Card.ImageUrl = temporaryImageUrl;
        Card.UploadedImage = null;
        Close();
    }

    [RelayCommand]
    public async Task UploadImage()
    {
        Card.UploadedImage = await _filesSevice.GetBitmap();
    }

    [RelayCommand]
    public void DeleteImage()
    {
        Card.UploadedImage = null;
        Card.ImageUrl = null;
    }
}
