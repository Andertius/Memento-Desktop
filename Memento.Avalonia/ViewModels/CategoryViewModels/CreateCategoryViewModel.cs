using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Memento.Avalonia.Data;
using Memento.Avalonia.HttpClients;
using Memento.Avalonia.Options;
using Memento.Avalonia.ViewModels.DialogViewModels;
using Microsoft.Extensions.Options;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Memento.Avalonia.ViewModels.CategoryViewModels;

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
        if (!Design.IsDesignMode)
        {
            throw new InvalidOperationException("Default constructor only allowed in design-time");
        }

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

    public Interaction<Unit, ImageData> OpenFile { get; } = new();

    [ReactiveCommand]
    public async Task SaveCategoryAsync()
    {
        var category = Category.ToDataModel();
        int id = await _client.AddCategory(category);

        if (Category.UploadedImage is not null && !String.IsNullOrWhiteSpace(Category.UploadedImageName))
        {
            using var stream = new MemoryStream();
            Category.UploadedImage.Save(stream);
            stream.Position = 0;

            string? fileName = await _client.UploadImage(id, Category.UploadedImageName, stream);
            Category.ImageUrl = $"{_options.Host}/{ApiPaths.CategoriesImagesPath}/{fileName}";
            Category.UploadedImage = null;
        }

        Category.Id = id;
        Close();
    }

    [ReactiveCommand]
    public void Cancel()
    {
        Category.UploadedImage = null;
        Close();
    }

    [ReactiveCommand]
    public async Task UploadImageAsync()
    {
        (Category.UploadedImage, Category.UploadedImageName) = await OpenFile.Handle(Unit.Default);
    }

    [ReactiveCommand]
    public void DeleteImage()
    {
        Category.UploadedImage = null;
        Category.ImageUrl = null;
    }
}
