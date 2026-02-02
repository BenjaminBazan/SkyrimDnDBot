using SkyrimDnDBot.Application.Commands.Interfaces;
using SkyrimDnDBot.Core.Interfaces;

namespace SkyrimDnDBot.Application.Commands;

public sealed class CommandRouter
{
    private readonly char _prefix;
    private readonly Dictionary<string, ICommandHandler> _handlers;

    public CommandRouter(char prefix, IEnumerable<ICommandHandler> handlers)
    {
        _prefix = prefix;
        _handlers = handlers.ToDictionary(h => h.Name, h => h, StringComparer.OrdinalIgnoreCase);
    }

    public async Task RouteAsync(IChatMessage message)
    {
        if (message.IsFromBot)
            return;
        
        var parsedCommand = CommandParser.TryParse(message.Content, _prefix);
        
        if (parsedCommand is null)
            return;
        
        if (_handlers.TryGetValue(parsedCommand.Name, out var handler))
        {
            await handler.HandleAsync(message, parsedCommand);
            return;
        }
        else
        {
            await message.ReplyAsync($"Unknown command '{parsedCommand.Name}'. Type '{_prefix}help' for a list of commands.");
            return;
        }
    }
}
