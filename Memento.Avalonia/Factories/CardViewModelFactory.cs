using Memento.Avalonia.HttpClients;
using Memento.Avalonia.Options;
using Memento.Avalonia.Services;
using Memento.Avalonia.ViewModels.CardViewModels;
using Microsoft.Extensions.Options;

namespace Memento.Avalonia.Factories;

public interface ICardViewModelFactory
{
    CreateCardViewModel CreateCreateCardViewModel();
    
    EditCardViewModel CreateEditCardViewModel(CardViewModel cardViewModel);
}

public sealed class CardViewModelFactory(
    ICardHttpClient _client,
    IDialogService _dialogService,
    IFilesService _filesService,
    IOptions<ApiClientOptions> _options) : ICardViewModelFactory
{
    public CreateCardViewModel CreateCreateCardViewModel()
        => new(_client, _filesService, _options);

    public EditCardViewModel CreateEditCardViewModel(CardViewModel cardViewModel)
        => new(_client, _filesService, _dialogService, _options, cardViewModel);
}
