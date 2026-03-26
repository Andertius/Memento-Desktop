using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Memento.Avalonia.Data;
using Memento.Avalonia.DataModels;
using Memento.Avalonia.HttpClients;
using Memento.Avalonia.Interfaces;
using Memento.Avalonia.Services;

namespace Memento.Avalonia.ViewModels.CardViewModels;

public partial class ManageCardsViewModel(
    ICardHttpClient _client,
    IDialogService _dialogService)
    : PageViewModel(ApplicationPageNames.ManageCards), IDialogProvider
{
    [ObservableProperty]
    private ObservableCollection<CardViewModel> _cards = [];

    [ObservableProperty]
    private DialogViewModel? _dialogViewModel;

    /// <summary>
    /// Design-time only constructor
    /// </summary>
    public ManageCardsViewModel()
        : this(null!, null!)
    {
    }

    public override async Task OnPageSelected()
    {
        var result = await _client.GetCards();
        Cards = new ObservableCollection<CardViewModel>(result.Select(CardViewModel.FromDataModel));
    }

    [RelayCommand]
    public async Task CreateCardAsync()
    {
        var viewModel = new CreateCardViewModel();
        await _dialogService.ShowDialogAsync(this, viewModel);

        if (viewModel.Canceled)
        {
            return;
        }

        var card = viewModel.Card.ToDataModel();
        int id = await _client.AddCard(card);

        card.Id = id;

        Cards.Add(CardViewModel.FromDataModel(card));
    }

    [RelayCommand]
    public async Task EditCard(CardViewModel cardViewModel)
    {
        var viewModel = new EditCardViewModel(cardViewModel);
        await _dialogService.ShowDialogAsync(this, viewModel);

        if (viewModel.Canceled)
        {
            return;
        }

        await _client.UpdateCard(viewModel.Card.ToDataModel());
    }
}
