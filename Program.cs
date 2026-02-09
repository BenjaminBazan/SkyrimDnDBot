using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Supabase;
using SkyrimDnDBot.Application.Commands;
using SkyrimDnDBot.Application.Commands.Handlers;
using SkyrimDnDBot.Application.Commands.Interfaces;
using SkyrimDnDBot.Application.Services;
using SkyrimDnDBot.Core.Configuration;
using SkyrimDnDBot.Core.Interfaces;
using SkyrimDnDBot.Infrastructure.Discord;
using SkyrimDnDBot.Infrastructure.Persistence;

class Program
{
    private DiscordSocketClient _client;
    private CommandRouter _commandRouter;

    static Task Main(string[] args) => new Program().MainAsync();

    public async Task MainAsync()
    {
        // Cargar configuración desde appsettings.json
        // Esto es el patrón estándar de .NET para manejar configuración
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables() // Las variables de entorno tienen prioridad
            .Build();

        var settings = configuration.Get<AppSettings>() 
            ?? throw new InvalidOperationException("No se pudo cargar la configuración");

        // Debug: verificar que el token se cargó correctamente
        Console.WriteLine($"🔍 Token cargado (primeros 20 chars): {settings.Discord.Token[..Math.Min(20, settings.Discord.Token.Length)]}...");
        Console.WriteLine($"🔍 Longitud del token: {settings.Discord.Token.Length} caracteres");

        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages | GatewayIntents.MessageContent
        });
        
        // Configurar el repositorio de personajes usando la configuración tipada
        var characterRepo = await CreateCharacterRepositoryAsync(settings);
        var characterService = new CharacterService(characterRepo);
        
        var handlers = new ICommandHandler[]
        {
            new PingHandler(),
            new HelpHandler(settings.Discord.CommandPrefix),
            new CharacterHandler(settings.Discord.CommandPrefix, characterService)
        };
        _commandRouter = new CommandRouter(settings.Discord.CommandPrefix, handlers);
        _client.Log += Log;
        _client.MessageReceived += MessageReceived;

        await _client.LoginAsync(TokenType.Bot, settings.Discord.Token);
        await _client.StartAsync();
     
        Console.WriteLine("Hoooola funcionando ando jeje");
        await Task.Delay(-1);

    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private async Task MessageReceived(SocketMessage message)
    {
        var chatMessage = new DiscordChatMessage(message);
        await _commandRouter.RouteAsync(chatMessage);
    }

    /// <summary>
    /// Factory method para crear el repositorio de personajes.
    /// 
    /// Ahora usa la configuración tipada en vez de variables de entorno directas.
    /// Esto hace que el código sea más testeable y mantenible.
    /// </summary>
    private static async Task<ICharacterRepository> CreateCharacterRepositoryAsync(AppSettings settings)
    {
        // Decisión basada en configuración: InMemory o Supabase
        if (settings.Persistence.Type.Equals("InMemory", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("💾 Usando almacenamiento en memoria (no persistente)");
            return new InMemoryCharacterRepository();
        }

        // Validar que tenemos credenciales de Supabase
        if (string.IsNullOrEmpty(settings.Supabase.Url) || string.IsNullOrEmpty(settings.Supabase.Key))
        {
            throw new InvalidOperationException(
                "Configuración de Supabase incompleta. Revisa appsettings.Development.json");
        }

        var options = new SupabaseOptions
        {
            AutoConnectRealtime = false
        };

        var supabaseClient = new Supabase.Client(settings.Supabase.Url, settings.Supabase.Key, options);
        await supabaseClient.InitializeAsync();

        Console.WriteLine("✅ Conectado a Supabase");
        return new SupabaseCharacterRepository(supabaseClient);
    }
}
