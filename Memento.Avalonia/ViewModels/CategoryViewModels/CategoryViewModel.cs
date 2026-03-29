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

namespace Memento.Avalonia.ViewModels.CategoryViewModels;

public partial class CategoryViewModel : ViewModelBase
{
    [Reactive]
    private int _id;

    [Reactive]
    private string? _name;

    [Reactive]
    private string? _description;

    [Reactive]
    private string? _imageUrl;

    [Reactive]
    private Bitmap? _uploadedImage;

    [Reactive]
    private ObservableCollection<Tag> _tags = [];

    [ObservableAsProperty]
    public Task<Bitmap?> _imageBitmap = Task.FromResult<Bitmap?>(null);

    [ObservableAsProperty]
    public string _combinedTags = "";
    
    public CategoryViewModel()
    {
        this.WhenAnyValue(x => x.UploadedImage, x => x.ImageUrl, CalculateImageBitmap)
            .ToProperty(this, x => x.ImageBitmap, out _imageBitmapHelper);
        
        this.WhenAnyValue(x => x.Tags, CalculateCombinedTags)
            .ToProperty(this, x => x.CombinedTags, out _combinedTagsHelper);
    }

    public string? UploadedImageName { get; set; }

    public static CategoryViewModel FromDataModel(Category category, string? imageUrl = null) => new()
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
    
    private static string CalculateCombinedTags(IEnumerable<Tag> tags)
        => String.Join(", ", tags.Select(x => x.Name));
}
