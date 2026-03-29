using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Memento.Avalonia.Data;
using Memento.Avalonia.HttpClients;
using Memento.Avalonia.Interfaces;
using Memento.Avalonia.Options;
using Memento.Avalonia.Services;
using Memento.Avalonia.ViewModels.DialogViewModels;
using Microsoft.Extensions.Options;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Memento.Avalonia.ViewModels.CategoryViewModels;

public partial class EditCategoryViewModel : DialogViewModelBase, IDialogProvider
{
    private readonly ApiClientOptions _options;
    private readonly ICategoryHttpClient _client;
    private readonly IDialogService _dialogService;
    private readonly string? _temporaryImageUrl;

    [Reactive]
    private CategoryViewModel _category;

    [Reactive]
    private DialogViewModelBase? _dialogViewModel;

    /// <summary>
    /// Design-time only constructor
    /// </summary>
    public EditCategoryViewModel()
    {
        if (!Design.IsDesignMode)
        {
            throw new InvalidOperationException("Default constructor only allowed in design-time");
        }

        _client = null!;
        _dialogService = null!;
        _options = null!;
        _category = new CategoryViewModel();
        _temporaryImageUrl = Category.ImageUrl;
    }

    public EditCategoryViewModel(
        ICategoryHttpClient client,
        IDialogService dialogService,
        IOptions<ApiClientOptions> options,
        CategoryViewModel category)
    {
        _client = client;
        _dialogService = dialogService;
        _options = options.Value;
        _category = category;
        _temporaryImageUrl = Category.ImageUrl;
    }

    public Interaction<Unit, ImageData> OpenFile { get; } = new();

    public bool Deleted { get; private set; }

    [ReactiveCommand]
    public async Task SaveCategoryAsync()
    {
        if (Category.UploadedImage is not null && !String.IsNullOrWhiteSpace(Category.UploadedImageName))
        {
            using var stream = new MemoryStream();
            Category.UploadedImage.Save(stream);
            stream.Position = 0;

            string? fileName = await _client.UploadImage(Category.Id, Category.UploadedImageName, stream);
            Category.ImageUrl = $"{_options.Host}/{ApiPaths.CategoriesImagesPath}/{fileName}";
            Category.UploadedImage = null;
        }
        else
        {
            await _client.DeleteImage(Category.Id);
        }

        await _client.UpdateCategory(Category.ToDataModel());
        Close();
    }

    [ReactiveCommand]
    public void Cancel()
    {
        Category.ImageUrl = _temporaryImageUrl;
        Category.UploadedImage = null;
        Close();
    }

    [ReactiveCommand]
    public async Task DeleteCategoryAsync()
    {
        var confirmViewModel = new DeleteConfirmationDialogViewModel { DeletedObjectName = Category.Name };
        await _dialogService.ShowDialogAsync(this, confirmViewModel);

        if (!confirmViewModel.Confirmed)
        {
            return;
        }

        await _client.DeleteCategory(Category.Id);

        Deleted = true;
        Close();
    }

    [ReactiveCommand]
    public async Task UploadImage()
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
