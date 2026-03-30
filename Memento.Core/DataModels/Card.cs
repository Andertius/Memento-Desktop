using System.Collections.Generic;

namespace Memento.Core.DataModels;

public sealed class Card : IEntity
{
    public int Id { get; set; }
    public string? Word { get; set; }
    public string? Translation { get; set; }
    public string? Definition { get; set; }
    public string? Hint { get; set; }
    public string? Image { get; set; }

    public IReadOnlyCollection<Category> Categories { get; set; } = [];
    public IReadOnlyCollection<Tag> Tags { get; set; } = [];
}
