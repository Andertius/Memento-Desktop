using Memento.Avalonia.HttpClients;
using Memento.Avalonia.Services;
using Memento.Avalonia.ViewModels.TagViewModels;

namespace Memento.Avalonia.Factories;

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
