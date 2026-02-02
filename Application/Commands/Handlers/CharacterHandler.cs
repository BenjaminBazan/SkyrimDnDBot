using SkyrimDnDBot.Application.Commands;
using SkyrimDnDBot.Application.Commands.Interfaces;
using SkyrimDnDBot.Application.Services;
using SkyrimDnDBot.Core.Interfaces;

namespace SkyrimDnDBot.Application.Commands.Handlers;

public sealed class CharacterHandler : ICommandHandler
{
    public string Name => "pj";

    private readonly char _prefix;
    private readonly CharacterService _characterService;

    public CharacterHandler(char prefix, CharacterService characterService)
    {
        _prefix = prefix;
        _characterService = characterService;
    }

    public async Task HandleAsync(IChatMessage message, ParsedCommand command)
    {
      if(command.Args.Length == 0)
        {
            await message.ReplyAsync(GetUsageText());
            return;
        }

        var sub = command.Args[0].ToLowerInvariant();

        try
        {
            switch (sub)
            {
                case "crear":
                    await HandleCreateAsync(message, command.Args);
                    break;
                case "ver":
                    await HandleViewAsync(message, command.Args);
                    break;
                default:
                    await message.ReplyAsync($"Subcomando desconocido '{sub}'.\n{GetUsageText()}");
                    break;
            }
        }catch(Exception ex)
        {
            await message.ReplyAsync($"Ocurrió un error al procesar el comando: {ex.Message}");
        }
    }
    private async Task HandleCreateAsync (IChatMessage message, string[] args)
    {
        if(args.Length < 4)
        {
            await message.ReplyAsync($"Uso incorrecto. Uso: '{_prefix}pj crear <nombre> <clase> <nivel>'");
            return;
        }

        var name = args[1];
        var @class = args[2];

        if (!int.TryParse(args[3], out var level) || level <= 0)
        {
            await message.ReplyAsync("Nivel inválido. Debe ser un número positivo.");
            return;
        }

        var c = await _characterService.CreateCharacterAsync(name, @class, level);
        await message.ReplyAsync($"Personaje '{c.Name}' ({c.Class}, Nivel {c.Level}) creado con éxito.");
    }

    private async Task HandleViewAsync (IChatMessage message, string[] args)
    {
        if(args.Length < 2)
        {
            await message.ReplyAsync($"Uso incorrecto. Uso: '{_prefix}pj ver <nombre>'");
            return;
        }

        var name = args[1];
        var c = await _characterService.GetCharacterByNameAsync(name);

        if(c is null)
        {
            await message.ReplyAsync($"No se encontró ningún personaje con el nombre '{name}'.");
            return;
        }

        await message.ReplyAsync($"Personaje: '{c.Name}' ({c.Class}, Nivel {c.Level})");
    }

    private string GetUsageText()
    {
        return "Comandos de personaje:\n" +
               $"{_prefix}pj crear <nombre> <clase> <nivel> - Crea un nuevo personaje.\n" +
               $"{_prefix}pj ver <nombre> - Muestra los detalles de un personaje existente.";
    }
}
