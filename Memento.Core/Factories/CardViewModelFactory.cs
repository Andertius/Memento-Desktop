using Memento.Core.HttpClients;
using Memento.Core.Options;
using Memento.Core.Services;
using Memento.Core.ViewModels.CardViewModels;
using Microsoft.Extensions.Options;

namespace Memento.Core.Factories;

public interface ICardViewModelFactory
{
    CreateCardViewModel CreateCreateCardViewModel();

    EditCardViewModel CreateEditCardViewModel(CardViewModel cardViewModel);
}

public sealed class CardViewModelFactory(
    ICardHttpClient _client,
    IDialogService _dialogService,
    IOptions<ApiClientOptions> _options) : ICardViewModelFactory
{
    public CreateCardViewModel CreateCreateCardViewModel()
        => new(_client, _options);

    public EditCardViewModel CreateEditCardViewModel(CardViewModel cardViewModel)
        => new(_client, _dialogService, _options, cardViewModel);
}
