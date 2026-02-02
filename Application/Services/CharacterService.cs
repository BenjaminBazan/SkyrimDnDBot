using SkyrimDnDBot.Core.Domain;
using SkyrimDnDBot.Core.Interfaces;

namespace SkyrimDnDBot.Application.Services;

public sealed class CharacterService
{
    private readonly ICharacterRepository _repository;

    public CharacterService(ICharacterRepository repository)
    {
        _repository = repository;
    }

    public async Task<Character> CreateCharacterAsync(string name, string @class, int level)
    {
        name = (name ?? "").Trim();
        @class = (@class ?? "").Trim();

        if(name.Length == 0)
            throw new ArgumentException("Character name cannot be empty.", nameof(name));
        if(@class.Length == 0)
            throw new ArgumentException("Character class cannot be empty.", nameof(@class));
        if(level < 1 || level > 20)
            throw new ArgumentOutOfRangeException(nameof(level), "Character level must be between 1 and 20.");

        var character = new Character(Guid.NewGuid(), name, @class, level);
        await _repository.SaveCharacterAsync(character);

        return character;
            
    }
    public Task<Character?> GetCharacterByNameAsync(string name)
    {
        name = (name ?? "").Trim();
        if(name.Length == 0)
            throw new ArgumentException("Character name cannot be empty.", nameof(name));

        return _repository.GetCharacterByNameAsync(name);
    }
}

