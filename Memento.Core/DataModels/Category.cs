using System.Collections.Generic;

namespace Memento.Core.DataModels;

public sealed class Category
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }

    public ICollection<Tag> Tags { get; set; } = [];
}
