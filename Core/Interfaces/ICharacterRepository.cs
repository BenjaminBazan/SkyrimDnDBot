using SkyrimDnDBot.Core.Domain;

namespace SkyrimDnDBot.Core.Interfaces;

public interface ICharacterRepository
{
    Task SaveCharacterAsync(Character character);
    Task<Character?> GetCharacterByNameAsync(string name);
}
