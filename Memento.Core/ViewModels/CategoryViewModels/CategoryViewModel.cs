using System;
using System.Collections.Generic;
using System.Linq;
using Memento.Core.Data;
using Memento.Core.DataModels;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels.CategoryViewModels;

public partial class CategoryViewModel : ViewModelBase
{
    [Reactive]
    private int _id;

    [Reactive]
    private string? _name;

    [Reactive]
    private string? _description;

    [Reactive]
    private Uri? _imageUrl;

    [Reactive]
    private ImageData? _uploadedImageData;

    [Reactive]
    private IReadOnlyCollection<Tag> _tags = [];

    [ObservableAsProperty]
    public string _combinedTags = "";

    public CategoryViewModel()
    {
        this.WhenAnyValue(x => x.Tags, CalculateCombinedTags)
            .ToProperty(this, x => x.CombinedTags, out _combinedTagsHelper);
    }

    public static CategoryViewModel FromDataModel(Category category, Uri? imageUrl = null) => new()
    {
        Id = category.Id,
        Name = category.Name,
        Description = category.Description,
        ImageUrl = imageUrl,
        Tags = category.Tags,
    };

    public Category ToDataModel() => new()
    {
        Id = Id,
        Name = Name,
        Description = Description,
        Tags = Tags,
    };

    private static string CalculateCombinedTags(IReadOnlyCollection<Tag> tags)
        => tags.Count == 0
            ? ""
            : "Tags: " + String.Join(", ", tags.Select(x => x.Name));
}
