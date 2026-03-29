using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Memento.Core.Data;
using Memento.Core.Factories;
using Memento.Core.HttpClients;
using Memento.Core.Interfaces;
using Memento.Core.Services;
using Memento.Core.ViewModels.DialogViewModels;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels.TagViewModels;

public partial class ManageTagsViewModel : PageViewModel, IDialogProvider
{
    [Reactive]
    private ObservableCollection<TagViewModel> _tags = [];

    [Reactive]
    private DialogViewModelBase? _dialogViewModel;

    private readonly ITagHttpClient _client;
    private readonly ITagViewModelFactory _tagViewModelFactory;
    private readonly IDialogService _dialogService;

    /// <summary>
    /// Design-time only constructor
    /// </summary>
    public ManageTagsViewModel()
        : base(ApplicationPageNames.ManageTags)
    {
        _client = null!;
        _dialogService = null!;
        _tagViewModelFactory = null!;
    }

    public ManageTagsViewModel(
        ITagHttpClient client,
        ITagViewModelFactory tagViewModelFactory,
        IDialogService dialogService)
        : base(ApplicationPageNames.ManageTags)
    {
        _client = client;
        _tagViewModelFactory = tagViewModelFactory;
        _dialogService = dialogService;
    }

    public override async Task OnPageSelected()
    {
        var result = await _client.GetTags();
        Tags = new ObservableCollection<TagViewModel>(result.Select(TagViewModel.FromDataModel));
    }

    [ReactiveCommand]
    public async Task CreateTagAsync()
    {
        var viewModel = _tagViewModelFactory.CreateCreateTagViewModel();
        await _dialogService.ShowDialogAsync(this, viewModel);

        if (viewModel.Tag.Id != 0)
        {
            Tags.Add(viewModel.Tag);
        }
    }

    [ReactiveCommand]
    public async Task EditTagAsync(TagViewModel cardViewModel)
    {
        var viewModel = _tagViewModelFactory.CreateEditTagViewModel(cardViewModel);
        await _dialogService.ShowDialogAsync(this, viewModel);

        if (viewModel.Deleted)
        {
            Tags.Remove(viewModel.Tag);
        }
    }
}
