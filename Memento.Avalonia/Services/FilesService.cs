using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;

namespace Memento.Avalonia.Services;

public interface IFilesService
{
    Task<(Bitmap?, string?)> GetBitmap();
}

public sealed class FilesService(Window _traget) : IFilesService
{
    public async Task<(Bitmap?, string?)> GetBitmap()
    {
        var topLevel = TopLevel.GetTopLevel(_traget);

        if (topLevel is null)
        {
            return (null, null);
        }

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Choose a png file",
            AllowMultiple = false,
            FileTypeFilter = [FilePickerFileTypes.ImageAll],
        });

        if (files.Count == 0)
        {
            return (null, null);
        }

        var stream = await files[0].OpenReadAsync();

        return (new Bitmap(stream), files[0].Name);
    }
}
