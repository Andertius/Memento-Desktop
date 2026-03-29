using System;
using Memento.Core.Data;

namespace Memento.Core.Services;

public static class ImageHelper
{
    public static Uri? GenerateCardImageUrl(string? imageName, string host)
        => String.IsNullOrWhiteSpace(imageName)
            ? null
            : new Uri($"{host}/{ApiPaths.CardsImagesPath}/{imageName}");

    public static Uri? GenerateCategoryImageUrl(string? imageName, string host)
        => String.IsNullOrWhiteSpace(imageName)
            ? null
            : new Uri($"{host}/{ApiPaths.CategoriesImagesPath}/{imageName}");
}
