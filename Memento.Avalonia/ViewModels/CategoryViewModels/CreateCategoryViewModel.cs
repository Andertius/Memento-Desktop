using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Memento.Avalonia.Data;
using Memento.Avalonia.HttpClients;
using Memento.Avalonia.Options;
using Memento.Avalonia.Services;
using Memento.Avalonia.ViewModels.DialogViewModels;
using Microsoft.Extensions.Options;

namespace Memento.Avalonia.ViewModels.CategoryViewModels;

public partial class CreateCategoryViewModel : DialogViewModelBase
{
    private readonly ApiClientOptions _options;

    [ObservableProperty]
    private CategoryViewModel _category = new();

    private readonly ICategoryHttpClient _client;
    private readonly IFilesService _filesService;

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
        _filesService = null!;
        _options = null!;
    }

    public CreateCategoryViewModel(
        ICategoryHttpClient client,
        IFilesService filesService,
        IOptions<ApiClientOptions> options)
    {
        _client = client;
        _filesService = filesService;
        _options = options.Value;
    }

    [RelayCommand]
    public async Task SaveCategory()
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

    [RelayCommand]
    public void Cancel()
    {
        Category.UploadedImage = null;
        Close();
    }

    [RelayCommand]
    public async Task UploadImage()
    {
        (Category.UploadedImage, Category.UploadedImageName) = await _filesService.GetBitmap();
    }

    [RelayCommand]
    public void DeleteImage()
    {
        Category.UploadedImage = null;
        Category.ImageUrl = null;
    }
}
