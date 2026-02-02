using SkyrimDnDBot.Application.Commands;
using SkyrimDnDBot.Application.Commands.Interfaces;
using SkyrimDnDBot.Core.Interfaces;

namespace SkyrimDnDBot.Application.Commands.Handlers;

public sealed class HelpHandler : ICommandHandler
{
    public string Name => "help";
    private readonly char _prefix;

    public HelpHandler(char prefix)
    {
        _prefix = prefix;
    }

    public async Task HandleAsync(IChatMessage message, ParsedCommand command)
    {
        var helpText = "Available commands:\n" +
                       $"{_prefix}ping - Responds with Pong!\n" +
                       $"{_prefix}help - Shows this help message";
        await message.ReplyAsync(helpText);
    }
}