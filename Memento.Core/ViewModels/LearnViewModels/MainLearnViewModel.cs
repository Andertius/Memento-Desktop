using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Memento.Core.Data;
using Memento.Core.DataModels;
using Memento.Core.HttpClients;
using Memento.Core.Interfaces;
using Memento.Core.Interfaces.ViewModels.LearnViewModels;
using Memento.Core.Options;
using Memento.Core.Services;
using Memento.Core.ViewModels.DialogViewModels;
using Microsoft.Extensions.Options;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels.LearnViewModels;

public partial class MainLearnViewModel : PageViewModel, IMainLearnViewModel, IDialogProvider
{
    private readonly ICardHttpClient _cardHttpClient;
    private readonly ICategoryHttpClient _categoryHttpClient;
    private readonly ITagHttpClient _tagHttpClient;
    private readonly IDialogService _dialogService;
    private readonly ApiClientOptions _options;

    [Reactive]
    private IReadOnlyCollection<Tag> _tags = [];

    [Reactive]
    private IReadOnlyCollection<Category> _categories = [];

    [Reactive]
    private IReadOnlyList<Card> _cards = [];

    [Reactive]
    private Category? _selectedCategory;

    [Reactive]
    private IReadOnlyCollection<Tag> _selectedTags = [];

    [Reactive]
    private DialogViewModelBase? _dialogViewModel;

    [Reactive]
    private string? _error;

    [ObservableAsProperty]
    public string _combinedTags = "";

    public MainLearnViewModel(
        ICardHttpClient cardHttpClient,
        ICategoryHttpClient categoryHttpClient,
        ITagHttpClient tagHttpClient,
        IDialogService dialogService,
        IOptions<ApiClientOptions> options) : base(ApplicationPageNames.Learn)
    {
        _cardHttpClient = cardHttpClient;
        _categoryHttpClient = categoryHttpClient;
        _tagHttpClient = tagHttpClient;
        _dialogService = dialogService;
        _options = options.Value;

        var canStart = this.WhenAnyValue(
            x => x.SelectedCategory,
            x => x.SelectedTags,
            (category, selectedTags) => category is not null || selectedTags is { Count: > 0 });

        StartLearnCommand = ReactiveCommand.CreateFromTask(StartLearn, canStart);

        this.WhenAnyValue(x => x.SelectedTags, CalculateCombinedTags)
            .ToProperty(this, nameof(CombinedTags), out _combinedTagsHelper, scheduler: RxSchedulers.MainThreadScheduler);
    }

    public ReactiveCommand<Unit, Unit> StartLearnCommand { get; }

    public override async Task OnPageSelected()
    {
        Categories = await _categoryHttpClient.GetCategories();
        Tags = await _tagHttpClient.GetTags();
    }

    public async Task StartLearn()
    {
        Error = null;

        var cards = await _cardHttpClient.GetCards(SelectedCategory?.Id, SelectedTags.Select(x => x.Id).ToArray());
        var cardsEnumerable = cards.AsEnumerable();

        if (SelectedTags is { Count: > 0 })
        {
            cardsEnumerable = cardsEnumerable.Where(x => x.Tags.IntersectBy(SelectedTags.Select(x => x.Id), x => x.Id).Any());
        }

        Cards = cardsEnumerable.ToArray();

        if (Cards.Count == 0)
        {
            Error = "No cards for the specified filter were found";

            return;
        }

        var viewModel = new LearnDialogViewModel(Cards, _options);
        await _dialogService.ShowDialogAsync(this, viewModel);
    }

    private static string CalculateCombinedTags(IReadOnlyCollection<Tag> tags)
        => tags.Count == 0
            ? ""
            : String.Join(", ", tags.Select(x => x.Name));
}
