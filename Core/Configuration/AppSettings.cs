namespace SkyrimDnDBot.Core.Configuration;

/// <summary>
/// Configuración fuertemente tipada de la aplicación.
/// 
/// ¿Por qué esto es mejor que usar strings?
/// 1. IntelliSense: el IDE te ayuda con autocompletado
/// 2. Type Safety: el compilador detecta errores antes de ejecutar
/// 3. Refactoring: si cambias un nombre, el IDE lo cambia en todos lados
/// 
/// Esto sigue el patrón Options de .NET.
/// </summary>
public sealed class AppSettings
{
    public DiscordSettings Discord { get; set; } = new();
    public SupabaseSettings Supabase { get; set; } = new();
    public PersistenceSettings Persistence { get; set; } = new();
}

public sealed class DiscordSettings
{
    public string Token { get; set; } = string.Empty;
    public char CommandPrefix { get; set; } = '%';
}

public sealed class SupabaseSettings
{
    public string Url { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
}

public sealed class PersistenceSettings
{
    /// <summary>
    /// Tipo de persistencia: "InMemory" o "Supabase"
    /// </summary>
    public string Type { get; set; } = "InMemory";
}
