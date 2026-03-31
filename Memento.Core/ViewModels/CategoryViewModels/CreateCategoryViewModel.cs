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
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;

namespace Memento.Core.ViewModels.CategoryViewModels;

public partial class CreateCategoryViewModel : DialogViewModelBase, ICreateCategoryViewModel, IValidatableViewModel
{
    private readonly ApiClientOptions _options;
    private readonly ICategoryHttpClient _client;

    [Reactive]
    private CategoryViewModel _category = new();

    [Reactive]
    private IReadOnlyCollection<TagViewModel> _availableTags;

    public CreateCategoryViewModel(
        ICategoryHttpClient client,
        IOptions<ApiClientOptions> options,
        IEnumerable<TagViewModel> tags)
    {
        _client = client;
        _options = options.Value;
        _availableTags = tags.ToList();

        var nameValidation = this.WhenAnyValue(x => x.Category.Name, name => !String.IsNullOrWhiteSpace(name));
        var descriptionValidation = this.WhenAnyValue(x => x.Category.Description, description => !String.IsNullOrWhiteSpace(description));

        var canSave = nameValidation.CombineLatest(descriptionValidation).Select(notEmpty => notEmpty is { First: true, Second: true });
        SaveCategoryCommand = ReactiveCommand.CreateFromTask(SaveCategoryAsync, canSave);

        this.ValidationRule(
            viewModel => viewModel.Category.Name,
            nameValidation,
            "Name cannot be empty");

        this.ValidationRule(
            viewModel => viewModel.Category.Description,
            descriptionValidation,
            "Description cannot be empty");
    }

    public IValidationContext ValidationContext { get; } = new ValidationContext();

    public Interaction<Unit, ImageData?> OpenFile { get; } = new();

    public ReactiveCommand<Unit, Unit> SaveCategoryCommand { get; }

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
