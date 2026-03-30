namespace Memento.Core.DataModels;

public sealed class Tag : IEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
}
