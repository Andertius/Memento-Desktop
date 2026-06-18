using System.Collections.Generic;

namespace Memento.Core.Requests;

public sealed class CardRequest
{
    public int Id { get; set; }
    public string? Word { get; set; }
    public string? Translation { get; set; }
    public string? Definition { get; set; }
    public string? Hint { get; set; }

    public IReadOnlyCollection<int> CategoryIds { get; set; } = [];
    public IReadOnlyCollection<int> TagIds { get; set; } = [];
}
