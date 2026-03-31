using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Memento.Core.Data;
using Memento.Core.Factories;
using Memento.Core.HttpClients;
using Memento.Core.Interfaces.ViewModels.TagViewModels;
using Memento.Core.Services;
using Memento.Core.ViewModels.DialogViewModels;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels.TagViewModels;

public partial class ManageTagsViewModel(
    ITagHttpClient _client,
    ITagViewModelFactory _tagViewModelFactory,
    IDialogService _dialogService) : PageViewModel(ApplicationPageNames.ManageTags), IManageTagsViewModel
{
    [Reactive]
    private ObservableCollection<TagViewModel> _tags = [];

    [Reactive]
    private DialogViewModelBase? _dialogViewModel;

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
    public async Task EditTagAsync(TagViewModel tagViewModel)
    {
        var viewModel = _tagViewModelFactory.CreateEditTagViewModel(tagViewModel.Clone());
        await _dialogService.ShowDialogAsync(this, viewModel);

        if (viewModel.Canceled)
        {
            return;
        }

        if (viewModel.Deleted)
        {
            Tags.Remove(viewModel.Tag);

            return;
        }

        int index = Tags.IndexOf(tagViewModel);

        if (index != -1)
        {
            Tags[index] = viewModel.Tag;
        }
    }
}
