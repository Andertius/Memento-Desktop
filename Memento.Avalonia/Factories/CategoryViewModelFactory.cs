using Memento.Avalonia.HttpClients;
using Memento.Avalonia.Options;
using Memento.Avalonia.Services;
using Memento.Avalonia.ViewModels.CategoryViewModels;
using Microsoft.Extensions.Options;

namespace Memento.Avalonia.Factories;

public interface ICategoryViewModelFactory
{
    CreateCategoryViewModel CreateCreateCategoryViewModel();
    
    EditCategoryViewModel CreateEditCategoryViewModel(CategoryViewModel categoryViewModel);
}

public sealed class CategoryViewModelFactory(
    ICategoryHttpClient _client,
    IDialogService _dialogService,
    IOptions<ApiClientOptions> _options) : ICategoryViewModelFactory
{
    public CreateCategoryViewModel CreateCreateCategoryViewModel()
        => new(_client, _options);

    public EditCategoryViewModel CreateEditCategoryViewModel(CategoryViewModel categoryViewModel)
        => new(_client, _dialogService, _options, categoryViewModel);
}
