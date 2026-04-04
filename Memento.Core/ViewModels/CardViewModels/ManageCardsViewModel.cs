using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Memento.Core.Data;
using Memento.Core.Factories;
using Memento.Core.HttpClients;
using Memento.Core.Interfaces.ViewModels.CardViewModels;
using Memento.Core.Options;
using Memento.Core.Services;
using Memento.Core.ViewModels.CategoryViewModels;
using Memento.Core.ViewModels.DialogViewModels;
using Memento.Core.ViewModels.TagViewModels;
using Microsoft.Extensions.Options;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels.CardViewModels;

public partial class ManageCardsViewModel(
    ICardHttpClient _cardClient,
    ICategoryHttpClient _categoryClient,
    ITagHttpClient _tagClient,
    ICardViewModelFactory _cardViewModelFactory,
    IDialogService _dialogService,
    IOptions<ApiClientOptions> options)
    : PageViewModel(ApplicationPageNames.ManageCards), IManageCardsViewModel
{
    private readonly ApiClientOptions _options = options.Value;

    [Reactive]
    private ObservableCollection<CardViewModel> _cards = [];

    [Reactive]
    private IReadOnlyCollection<CategoryViewModel> _categories = [];

    [Reactive]
    private IReadOnlyCollection<TagViewModel> _tags = [];

    [Reactive]
    private DialogViewModelBase? _dialogViewModel;

    public override async Task OnPageSelected()
    {
        var cards = await _cardClient.GetAllCards();
        var categories = await _categoryClient.GetCategories();
        var tags = await _tagClient.GetTags();

        Cards = new ObservableCollection<CardViewModel>(cards.Select(x => CardViewModel.FromDataModel(x, ImageHelper.GenerateCardImageUrl(x.Image, _options.Host))));
        Categories = categories.Select(x => CategoryViewModel.FromDataModel(x, ImageHelper.GenerateCategoryImageUrl(x.Image, _options.Host))).ToList();
        Tags = tags.Select(TagViewModel.FromDataModel).ToList();
    }

    [ReactiveCommand]
    public async Task CreateCardAsync()
    {
        var viewModel = _cardViewModelFactory.CreateCreateCardViewModel(Categories, Tags);
        await _dialogService.ShowDialogAsync(this, viewModel);

        if (viewModel.Card.Id != 0)
        {
            Cards.Add(viewModel.Card);
        }
    }

    [ReactiveCommand]
    public async Task EditCardAsync(CardViewModel cardViewModel)
    {
        var viewModel = _cardViewModelFactory.CreateEditCardViewModel(cardViewModel.Clone(), Categories, Tags);
        await _dialogService.ShowDialogAsync(this, viewModel);

        if (viewModel.Canceled)
        {
            return;
        }

        if (viewModel.Deleted)
        {
            Cards.Remove(cardViewModel);

            return;
        }

        int index = Cards.IndexOf(cardViewModel);

        if (index != -1)
        {
            Cards[index] = viewModel.Card;
        }
    }
}
