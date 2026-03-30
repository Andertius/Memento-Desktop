using System;
using System.Collections.Generic;
using System.Linq;
using Memento.Core.Data;
using Memento.Core.DataModels;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Memento.Core.ViewModels.CardViewModels;

public partial class CardViewModel : ViewModelBase
{
    [Reactive]
    private int _id;

    [Reactive]
    private string? _word;

    [Reactive]
    private string? _translation;

    [Reactive]
    private string? _definition;

    [Reactive]
    private string? _hint;

    [Reactive]
    private Uri? _imageUrl;

    [Reactive]
    private ImageData? _uploadedImageData;

    [Reactive]
    private IReadOnlyCollection<Category> _categories = [];

    [Reactive]
    private IReadOnlyCollection<Tag> _tags = [];

    [ObservableAsProperty]
    public string _combinedCategories = "";

    [ObservableAsProperty]
    public string _combinedTags = "";

    public CardViewModel()
    {
        this.WhenAnyValue(x => x.Categories, CalculateCombinedCategories)
            .ToProperty(this, nameof(CombinedCategories), out _combinedCategoriesHelper, scheduler: RxSchedulers.MainThreadScheduler);

        this.WhenAnyValue(x => x.Tags, CalculateCombinedTags)
            .ToProperty(this, nameof(CombinedTags), out _combinedTagsHelper, scheduler: RxSchedulers.MainThreadScheduler);
    }

    public static CardViewModel FromDataModel(Card card, Uri? imageUrl = null) => new()
    {
        Id = card.Id,
        Word = card.Word,
        Translation = card.Translation,
        Definition = card.Definition,
        Hint = card.Hint,
        ImageUrl = imageUrl,
        Categories = card.Categories,
        Tags = card.Tags,
    };

    public Card ToDataModel() => new()
    {
        Id = Id,
        Word = Word,
        Translation = Translation,
        Definition = Definition,
        Hint = Hint,
        Categories = Categories,
        Tags = Tags,
    };

    private static string CalculateCombinedCategories(IReadOnlyCollection<Category> categories)
        => categories.Count == 0
            ? ""
            : "Categories: " + String.Join(", ", categories.Select(x => x.Name));

    private static string CalculateCombinedTags(IReadOnlyCollection<Tag> tags)
        => tags.Count == 0
            ? ""
            : "Tags: " + String.Join(", ", tags.Select(x => x.Name));
}
