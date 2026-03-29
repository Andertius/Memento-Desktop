using Memento.Core.HttpClients;
using Memento.Core.Services;
using Memento.Core.ViewModels.TagViewModels;

namespace Memento.Core.Factories;

public interface ITagViewModelFactory
{
    CreateTagViewModel CreateCreateTagViewModel();

    EditTagViewModel CreateEditTagViewModel(TagViewModel tagViewModel);
}

public sealed class TagViewModelFactory(
    ITagHttpClient _client,
    IDialogService _dialogService) : ITagViewModelFactory
{
    public CreateTagViewModel CreateCreateTagViewModel()
        => new(_client);

    public EditTagViewModel CreateEditTagViewModel(TagViewModel tagViewModel)
        => new(_client, _dialogService, tagViewModel);
}
