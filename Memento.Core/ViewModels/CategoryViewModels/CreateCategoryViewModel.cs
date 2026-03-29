using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Memento.Core.Data;
using Memento.Core.HttpClients;
using Memento.Core.Options;
using Memento.Core.ViewModels.DialogViewModels;
using Microsoft.Extensions.Options;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels.CategoryViewModels;

public partial class CreateCategoryViewModel : DialogViewModelBase
{
    private readonly ApiClientOptions _options;

    [Reactive]
    private CategoryViewModel _category = new();

    private readonly ICategoryHttpClient _client;

    /// <summary>
    /// Design-time only constructor
    /// </summary>
    public CreateCategoryViewModel()
    {
        _client = null!;
        _options = null!;
    }

    public CreateCategoryViewModel(
        ICategoryHttpClient client,
        IOptions<ApiClientOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

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
