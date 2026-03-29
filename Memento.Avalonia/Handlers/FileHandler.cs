using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Memento.Avalonia.Data;

namespace Memento.Avalonia.Handlers;

public static class FileHandler
{
    public static async Task<ImageData> OpenImage(Visual target)
    {
        var topLevel = TopLevel.GetTopLevel(target);

        if (topLevel is null)
        {
            return new ImageData(null, null);
        }

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Choose an image file",
            AllowMultiple = false,
            FileTypeFilter = [FilePickerFileTypes.ImageAll],
        });

        if (files.Count == 0)
        {
            return new ImageData(null, null);
        }

        var stream = await files[0].OpenReadAsync();

        return new ImageData(new Bitmap(stream), files[0].Name);
    }
}
