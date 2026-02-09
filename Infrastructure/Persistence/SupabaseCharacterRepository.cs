using Postgrest.Attributes;
using Postgrest.Models;
using Supabase;
using SkyrimDnDBot.Core.Domain;
using SkyrimDnDBot.Core.Interfaces;

namespace SkyrimDnDBot.Infrastructure.Persistence;

/// <summary>
/// Implementación de ICharacterRepository usando Supabase como almacenamiento cloud.
/// Nota: Esta clase sigue el mismo contrato que InMemoryCharacterRepository,
/// demostrando el principio Open/Closed - puedes cambiar la implementación
/// sin modificar el código que la consume.
/// </summary>
public sealed class SupabaseCharacterRepository : ICharacterRepository
{
    private readonly Supabase.Client _supabaseClient;
    private const string TableName = "characters";

    public SupabaseCharacterRepository(Supabase.Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public async Task SaveCharacterAsync(Character character)
    {
        var dto = CharacterDto.FromDomain(character);
        
        // Upsert: inserta si no existe, actualiza si existe (basado en id)
        await _supabaseClient
            .From<CharacterDto>()
            .Upsert(dto);
    }

    public async Task<Character?> GetCharacterByNameAsync(string name)
    {
        var response = await _supabaseClient
            .From<CharacterDto>()
            .Where(c => c.Name == name)
            .Single();

        return response?.ToDomain();
    }
}

/// <summary>
/// DTO (Data Transfer Object) para mapear entre el dominio y Supabase.
/// 
/// ¿Por qué un DTO separado?
/// 1. El dominio (Character) no debe conocer detalles de persistencia
/// 2. Supabase necesita atributos específicos para el mapeo
/// 3. Si la estructura de la BD cambia, solo cambias el DTO, no el dominio
/// 
/// Esto sigue el principio de Single Responsibility (SRP).
/// </summary>
[Table("characters")]
internal sealed class CharacterDto : BaseModel
{
    [PrimaryKey("id", false)]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("class")]
    public string Class { get; set; } = string.Empty;

    [Column("level")]
    public int Level { get; set; }

    /// <summary>
    /// Convierte del dominio al DTO (para guardar)
    /// </summary>
    public static CharacterDto FromDomain(Character character) => new()
    {
        Id = character.Id,
        Name = character.Name,
        Class = character.Class,
        Level = character.Level
    };

    /// <summary>
    /// Convierte del DTO al dominio (para leer)
    /// </summary>
    public Character ToDomain() => new(Id, Name, Class, Level);
}
