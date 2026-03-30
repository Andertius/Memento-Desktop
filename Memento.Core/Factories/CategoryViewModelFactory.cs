using System.Collections.Generic;
using Memento.Core.HttpClients;
using Memento.Core.Options;
using Memento.Core.Services;
using Memento.Core.ViewModels.CategoryViewModels;
using Memento.Core.ViewModels.TagViewModels;
using Microsoft.Extensions.Options;

namespace Memento.Core.Factories;

public interface ICategoryViewModelFactory
{
    CreateCategoryViewModel CreateCreateCategoryViewModel(IReadOnlyCollection<TagViewModel> tags);

    EditCategoryViewModel CreateEditCategoryViewModel(CategoryViewModel categoryViewModel, IReadOnlyCollection<TagViewModel> tags);
}

public sealed class CategoryViewModelFactory(
    ICategoryHttpClient _client,
    IDialogService _dialogService,
    IOptions<ApiClientOptions> _options) : ICategoryViewModelFactory
{
    public CreateCategoryViewModel CreateCreateCategoryViewModel(IReadOnlyCollection<TagViewModel> tags)
        => new(_client, _options, tags);

    public EditCategoryViewModel CreateEditCategoryViewModel(CategoryViewModel categoryViewModel, IReadOnlyCollection<TagViewModel> tags)
        => new(_client, _dialogService, _options, categoryViewModel, tags);
}
