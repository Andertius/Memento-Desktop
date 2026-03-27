using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using Memento.Avalonia.DataModels;
using Memento.Avalonia.Services;

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
    private string? _imageUrl;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ImageBitmap))]
    private Bitmap? _uploadedImage;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CombinedCategories))]
    private ObservableCollection<Category> _categories = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CombinedTags))]
    private ObservableCollection<Tag> _tags = [];

    public string CombinedCategories => String.Join(", ", Categories.Select(x => x.Name));

    public string CombinedTags => String.Join(", ", Tags.Select(x => x.Name));

    public Task<Bitmap?> ImageBitmap
    {
        get
        {
            if (UploadedImage is null)
            {
                return ImageUrl is null
                    ? Task.FromResult<Bitmap?>(null)
                    : ImageHelper.LoadFromWeb(new Uri(ImageUrl));
            }

            return Task.FromResult<Bitmap?>(UploadedImage);
        }
    }

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
}
