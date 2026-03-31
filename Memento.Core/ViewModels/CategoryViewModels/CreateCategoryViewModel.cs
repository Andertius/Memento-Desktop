using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Memento.Core.Data;
using Memento.Core.HttpClients;
using Memento.Core.Interfaces.ViewModels.CategoryViewModels;
using Memento.Core.Options;
using Memento.Core.ViewModels.DialogViewModels;
using Memento.Core.ViewModels.TagViewModels;
using Microsoft.Extensions.Options;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels.CategoryViewModels;

public partial class CreateCategoryViewModel(
    ICategoryHttpClient _client,
    IOptions<ApiClientOptions> options,
    IEnumerable<TagViewModel> tags)
    : DialogViewModelBase, ICreateCategoryViewModel
{
    private readonly ApiClientOptions _options = options.Value;

    [Reactive]
    private CategoryViewModel _category = new();

    [Reactive]
    private IReadOnlyCollection<TagViewModel> _availableTags = tags.ToList();

    public Interaction<Unit, ImageData?> OpenFile { get; } = new();

    [ReactiveCommand]
    public async Task SaveCategoryAsync()
    {
        var category = Category.ToDataModel();
        int id = await _client.AddCategory(category);

        if (Category.UploadedImageData is not null)
        {
            string? fileName = await _client.UploadImage(id, Category.UploadedImageData);
            Category.ImageUrl = new Uri($"{_options.Host}/{ApiPaths.CategoriesImagesPath}/{fileName}");
            Category.UploadedImageData = null;
        }

        Category.Id = id;
        Close();
    }

    [ReactiveCommand]
    public void Cancel()
    {
        Category.UploadedImageData = null;
        Close();
    }

    [ReactiveCommand]
    public async Task UploadImageAsync()
    {
        Category.UploadedImageData = await OpenFile.Handle(Unit.Default);

        if (Category.UploadedImageData is { FilePath: not null })
        {
            Category.ImageUrl = Category.UploadedImageData.FilePath;
        }
    }

    [ReactiveCommand]
    public void DeleteImage()
    {
        Category.UploadedImageData = null;
        Category.ImageUrl = null;
    }
}
