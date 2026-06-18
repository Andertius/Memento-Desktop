using System.Linq;
using Memento.Core.DataModels;
using Memento.Core.Requests;

namespace Memento.Core.Mappers;

public static class CardMapper
{
    public static CardRequest ToRequest(this Card card) => new()
    {
        Id = card.Id,
        Word = card.Word,
        Translation = card.Translation,
        Definition = card.Definition,
        Hint = card.Hint,
        CategoryIds = card.Categories.Select(x => x.Id).ToArray(),
        TagIds = card.Tags.Select(x => x.Id).ToArray(),
    };
}
