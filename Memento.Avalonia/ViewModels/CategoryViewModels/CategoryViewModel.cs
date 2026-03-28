using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using Memento.Avalonia.DataModels;
using Memento.Avalonia.Services;

namespace Memento.Avalonia.ViewModels.CategoryViewModels;

public partial class CategoryViewModel : ViewModelBase
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private string? _description;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ImageBitmap))]
    private string? _imageUrl;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ImageBitmap))]
    private Bitmap? _uploadedImage;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CombinedTags))]
    private ObservableCollection<Tag> _tags = [];
    
    public string? UploadedImageName { get; set; }

    public string CombinedTags => String.Join(", ", Tags.Select(x => x.Name));

    public Task<Bitmap?> ImageBitmap
    {
        get
        {
            if (UploadedImage is null)
            {
                return ImageUrl is null
                    ? Task.FromResult<Bitmap?>(null)
                    : ImageHelper.LoadFromServer(new Uri(ImageUrl));
            }

            return Task.FromResult<Bitmap?>(UploadedImage);
        }
    }

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
}
