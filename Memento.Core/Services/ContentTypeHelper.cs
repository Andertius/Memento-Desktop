using System;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace Memento.Core.Services;

public static class ContentTypeHelper
{
    public static MediaTypeHeaderValue FileExtensionToMediaTypeHeaderValue(string extension)
    {
        string contentType = extension switch
        {
            ".avif" => MediaTypeNames.Image.Avif,
            ".bmp" => MediaTypeNames.Image.Bmp,
            ".gif" => MediaTypeNames.Image.Gif,
            ".ico" => MediaTypeNames.Image.Icon,
            ".jpg" or ".jpeg" => MediaTypeNames.Image.Jpeg,
            ".png" => MediaTypeNames.Image.Png,
            ".svg" => MediaTypeNames.Image.Svg,
            ".tff" or ".tiff" => MediaTypeNames.Image.Tiff,
            ".webp" => MediaTypeNames.Image.Webp,
            _ => throw new ArgumentException($"Image format '{extension}' is not supported.", nameof(extension))
        };

        return MediaTypeHeaderValue.Parse(contentType);
    }
}
