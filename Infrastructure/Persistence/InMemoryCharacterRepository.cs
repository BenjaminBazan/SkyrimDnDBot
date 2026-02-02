using SkyrimDnDBot.Core.Domain;
using SkyrimDnDBot.Core.Interfaces;

namespace SkyrimDnDBot.Infrastructure.Persistence;

public sealed class InMemoryCharacterRepository : ICharacterRepository
{
    private readonly Dictionary<string, Character> _store = new(StringComparer.OrdinalIgnoreCase);

    public Task SaveCharacterAsync(Character character)
    {
        _store[character.Name] = character;
        return Task.CompletedTask;
    }

    public Task<Character?> GetCharacterByNameAsync(string name)
    {
        _store.TryGetValue(name, out var character);
        return Task.FromResult(character);
    }
}