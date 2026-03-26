using System.Collections.Generic;

namespace Memento.Avalonia.DataModels;

public sealed class Card
{
    public int Id { get; set; }
    public string? Word { get; set; }
    public string? Translation { get; set; }
    public string? Definition { get; set; }
    public string? Hint { get; set; }
    public string? Image { get; set; }

    public ICollection<Category> Categories { get; set; } = [];
    public ICollection<Tag> Tags { get; set; } = [];
}
