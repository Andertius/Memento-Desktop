using System.Collections.Generic;

namespace Memento.Core.Requests;

public sealed class CategoryRequest
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }

    public IReadOnlyCollection<int> TagIds { get; set; } = [];
}
