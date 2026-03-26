using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using Memento.Avalonia.DataModels;

namespace Memento.Avalonia.ViewModels.CardViewModels;

public partial class CardViewModel : ViewModelBase
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string? _word;

    [ObservableProperty]
    private string? _translation;

    [ObservableProperty]
    private string? _definition;

    [ObservableProperty]
    private string? _hint;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ImageBitmap))]
    private string? _image;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CombinedCategories))]
    private ObservableCollection<Category> _categories = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CombinedTags))]
    private ObservableCollection<Tag> _tags = [];

    public string CombinedCategories => String.Join(", ", Categories.Select(x => x.Name));

    public string CombinedTags => String.Join(", ", Tags.Select(x => x.Name));

    public Bitmap ImageBitmap => new(AssetLoader.Open(new Uri("avares://Memento.Avalonia/Assets/Spaghet.png")));

    public static CardViewModel FromDataModel(Card card) => new()
    {
        Id = card.Id,
        Word = card.Word,
        Translation = card.Translation,
        Definition = card.Definition,
        Hint = card.Hint,
        Image = card.Image,
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
        Image = Image,
        Categories = Categories.ToList(),
        Tags = Tags.ToList(),
    };
}
