using System.Collections.Generic;

namespace Memento.Core.DataModels;

public sealed class Category : IEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }

    public IReadOnlyCollection<Tag> Tags { get; set; } = [];
}
