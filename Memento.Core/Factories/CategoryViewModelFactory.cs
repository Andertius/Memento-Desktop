using Memento.Core.HttpClients;
using Memento.Core.Options;
using Memento.Core.Services;
using Memento.Core.ViewModels.CategoryViewModels;
using Microsoft.Extensions.Options;

namespace Memento.Core.Factories;

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
