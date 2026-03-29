using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Memento.Core.Data;
using Memento.Core.HttpClients;
using Memento.Core.Interfaces;
using Memento.Core.Options;
using Memento.Core.Services;
using Memento.Core.ViewModels.DialogViewModels;
using Microsoft.Extensions.Options;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels.CategoryViewModels;

public partial class EditCategoryViewModel : DialogViewModelBase, IDialogProvider
{
    private readonly ApiClientOptions _options;
    private readonly ICategoryHttpClient _client;
    private readonly IDialogService _dialogService;
    private readonly Uri? _temporaryImageUrl;

    [Reactive]
    private CategoryViewModel _category;

    [Reactive]
    private DialogViewModelBase? _dialogViewModel;

    /// <summary>
    /// Design-time only constructor
    /// </summary>
    public EditCategoryViewModel()
    {
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

    public Interaction<Unit, ImageData?> OpenFile { get; } = new();

    public bool Deleted { get; private set; }

    [ReactiveCommand]
    public async Task SaveCategoryAsync()
    {
        if (Category.UploadedImageData is not null)
        {
            string? fileName = await _client.UploadImage(Category.Id, Category.UploadedImageData);
            Category.ImageUrl = new Uri($"{_options.Host}/{ApiPaths.CategoriesImagesPath}/{fileName}");
            Category.UploadedImageData = null;
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
        Category.UploadedImageData = null;
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
