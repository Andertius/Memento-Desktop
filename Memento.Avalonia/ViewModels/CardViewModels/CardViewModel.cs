using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Memento.Avalonia.DataModels;
using Memento.Avalonia.Services;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Memento.Avalonia.ViewModels.CardViewModels;

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
    private string? _imageUrl;

    [Reactive]
    private Bitmap? _uploadedImage;

    [Reactive]
    private ObservableCollection<Category> _categories = [];

    [Reactive]
    private ObservableCollection<Tag> _tags = [];

    [ObservableAsProperty]
    public Task<Bitmap?> _imageBitmap = Task.FromResult<Bitmap?>(null);

    [ObservableAsProperty]
    public string _combinedCategories = "";

    [ObservableAsProperty]
    public string _combinedTags = "";
    
    public CardViewModel()
    {
        this.WhenAnyValue(x => x.UploadedImage, x => x.ImageUrl, CalculateImageBitmap)
            .ToProperty(this, x => x.ImageBitmap, out _imageBitmapHelper);
        
        this.WhenAnyValue(x => x.Categories, CalculateCombinedCategories)
            .ToProperty(this, x => x.CombinedTags, out _combinedCategoriesHelper);
        
        this.WhenAnyValue(x => x.Tags, CalculateCombinedTags)
            .ToProperty(this, x => x.CombinedTags, out _combinedTagsHelper);
    }

    public string? UploadedImageName { get; set; }

    public static CardViewModel FromDataModel(Card card, string? imageUrl = null) => new()
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

    private static Task<Bitmap?> CalculateImageBitmap(Bitmap? uploadedImage, string? imageUrl)
    {
        if (uploadedImage is null)
        {
            return imageUrl is null
                ? Task.FromResult<Bitmap?>(null)
                : ImageHelper.LoadFromServer(new Uri(imageUrl));
        }

        return Task.FromResult<Bitmap?>(uploadedImage);
    }
    
    private static string CalculateCombinedCategories(IEnumerable<Category> categories)
        => String.Join(", ", categories.Select(x => x.Name));

    private static string CalculateCombinedTags(IEnumerable<Tag> tags)
        => String.Join(", ", tags.Select(x => x.Name));
}
