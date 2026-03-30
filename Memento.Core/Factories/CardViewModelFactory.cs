using System.Collections.Generic;
using Memento.Core.HttpClients;
using Memento.Core.Options;
using Memento.Core.Services;
using Memento.Core.ViewModels.CardViewModels;
using Memento.Core.ViewModels.CategoryViewModels;
using Memento.Core.ViewModels.TagViewModels;
using Microsoft.Extensions.Options;

namespace Memento.Core.Factories;

public interface ICardViewModelFactory
{
    CreateCardViewModel CreateCreateCardViewModel(IReadOnlyCollection<CategoryViewModel> categories, IReadOnlyCollection<TagViewModel> tags);

    EditCardViewModel CreateEditCardViewModel(CardViewModel cardViewModel, IReadOnlyCollection<CategoryViewModel> categories, IReadOnlyCollection<TagViewModel> tags);
}

public sealed class CardViewModelFactory(
    ICardHttpClient _client,
    IDialogService _dialogService,
    IOptions<ApiClientOptions> _options) : ICardViewModelFactory
{
    public CreateCardViewModel CreateCreateCardViewModel(IReadOnlyCollection<CategoryViewModel> categories, IReadOnlyCollection<TagViewModel> tags)
        => new(_client, _options, categories, tags);

    public EditCardViewModel CreateEditCardViewModel(CardViewModel cardViewModel, IReadOnlyCollection<CategoryViewModel> categories, IReadOnlyCollection<TagViewModel> tags)
        => new(_client, _dialogService, _options, cardViewModel, categories, tags);
}
