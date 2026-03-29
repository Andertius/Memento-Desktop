using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    private ObservableCollection<Tag> _tags = [];

    [ObservableAsProperty]
    public string _combinedTags = "";

    public CategoryViewModel()
    {
        this.WhenAnyValue<CategoryViewModel, string, ObservableCollection<Tag>>(x => x.Tags, CalculateCombinedTags)
            .ToProperty(this, x => x.CombinedTags, out _combinedTagsHelper);
    }

    public static CategoryViewModel FromDataModel(Category category, Uri? imageUrl = null) => new()
    {
        Id = category.Id,
        Name = category.Name,
        Description = category.Description,
        ImageUrl = imageUrl,
        Tags = new ObservableCollection<Tag>(category.Tags),
    };

    public Category ToDataModel() => new()
    {
        Id = Id,
        Name = Name,
        Description = Description,
        Tags = Tags.ToList(),
    };

    private static string CalculateCombinedTags(IEnumerable<Tag> tags)
        => String.Join(", ", tags.Select(x => x.Name));
}
