using System.Linq;
using Memento.Core.DataModels;
using Memento.Core.Requests;

namespace Memento.Core.Mappers;

public static class CategoryMapper
{
    public static CategoryRequest ToRequest(this Category category) => new()
    {
        Id = category.Id,
        Name = category.Name,
        Description = category.Description,
        Image = category.Image,
        TagIds = category.Tags.Select(x => x.Id).ToArray(),
    };
}
