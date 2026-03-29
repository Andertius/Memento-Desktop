using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    private ObservableCollection<Category> _categories = [];

    [Reactive]
    private ObservableCollection<Tag> _tags = [];

    [ObservableAsProperty]
    public string _combinedCategories = "";

    [ObservableAsProperty]
    public string _combinedTags = "";

    public CardViewModel()
    {
        this.WhenAnyValue<CardViewModel, string, ObservableCollection<Category>>(x => x.Categories, CalculateCombinedCategories)
            .ToProperty(this, x => x.CombinedTags, out _combinedCategoriesHelper);

        this.WhenAnyValue<CardViewModel, string, ObservableCollection<Tag>>(x => x.Tags, CalculateCombinedTags)
            .ToProperty(this, x => x.CombinedTags, out _combinedTagsHelper);
    }

    public static CardViewModel FromDataModel(Card card, Uri? imageUrl = null) => new()
    {
        Id = card.Id,
        Word = card.Word,
        Translation = card.Translation,
        Definition = card.Definition,
        Hint = card.Hint,
        ImageUrl = imageUrl,
        Categories = new ObservableCollection<Category>(card.Categories),
        Tags = new ObservableCollection<Tag>(card.Tags),
    };

    public Card ToDataModel() => new()
    {
        Id = Id,
        Word = Word,
        Translation = Translation,
        Definition = Definition,
        Hint = Hint,
        Categories = Categories.ToList(),
        Tags = Tags.ToList(),
    };

    private static string CalculateCombinedCategories(IEnumerable<Category> categories)
        => String.Join(", ", categories.Select(x => x.Name));

    private static string CalculateCombinedTags(IEnumerable<Tag> tags)
        => String.Join(", ", tags.Select(x => x.Name));
}
