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
using Memento.Core.Services;
using Memento.Core.ViewModels.DialogViewModels;
using Memento.Core.ViewModels.TagViewModels;
using Microsoft.Extensions.Options;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;

namespace Memento.Core.ViewModels.CategoryViewModels;

public partial class EditCategoryViewModel : DialogViewModelBase, IEditCategoryViewModel, IValidatableViewModel
{
    private readonly ApiClientOptions _options;
    private readonly ICategoryHttpClient _client;
    private readonly IDialogService _dialogService;
    private readonly Uri? _temporaryImageUrl;

    [Reactive]
    private CategoryViewModel _category;

    [Reactive]
    private DialogViewModelBase? _dialogViewModel;

    [Reactive]
    private IReadOnlyCollection<TagViewModel> _availableTags;

    public EditCategoryViewModel(
        ICategoryHttpClient client,
        IDialogService dialogService,
        IOptions<ApiClientOptions> options,
        CategoryViewModel category,
        IReadOnlyCollection<TagViewModel> tags)
    {
        _client = client;
        _dialogService = dialogService;
        _options = options.Value;
        _category = category;
        _temporaryImageUrl = Category.ImageUrl;
        _availableTags = tags;

        var wordValidation = this.WhenAnyValue(x => x.Category.Name, name => !String.IsNullOrWhiteSpace(name));
        var translationValidation = this.WhenAnyValue(x => x.Category.Description, description => !String.IsNullOrWhiteSpace(description));

        var canSave = wordValidation.CombineLatest(translationValidation).Select(notEmpty => notEmpty is { First: true, Second: true });
        SaveCategoryCommand = ReactiveCommand.CreateFromTask(SaveCategoryAsync, canSave);

        this.ValidationRule(
            viewModel => viewModel.Category.Name,
            wordValidation,
            "Name cannot be empty");

        this.ValidationRule(
            viewModel => viewModel.Category.Description,
            translationValidation,
            "Description cannot be empty");
    }

    public IValidationContext ValidationContext { get; } = new ValidationContext();

    public Interaction<Unit, ImageData?> OpenFile { get; } = new();

    public ReactiveCommand<Unit, Unit> SaveCategoryCommand { get; }

    public bool Deleted { get; private set; }

    public bool Canceled { get; private set; }

    public async Task SaveCategoryAsync()
    {
        if (Category.UploadedImageData is not null)
        {
            string? fileName = await _client.UploadImage(Category.Id, Category.UploadedImageData);
            Category.ImageUrl = new Uri($"{_options.Host}/{ApiPaths.CategoriesImagesPath}/{fileName}");
            Category.UploadedImageData = null;
        }
        else if (Category.ImageUrl is null)
        {
            await _client.DeleteImage(Category.Id);
        }

        await _client.UpdateCategory(Category.ToDataModel());
        await _client.UpdateCategoryTags(Category.Id, Category.Tags.Select(x => x.Id).ToArray());
        Close();
    }

    [ReactiveCommand]
    public void Cancel()
    {
        Category.ImageUrl = _temporaryImageUrl;
        Category.UploadedImageData = null;

        Canceled = true;
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
