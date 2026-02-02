using SkyrimDnDBot.Application.Commands;
using SkyrimDnDBot.Application.Commands.Interfaces;
using SkyrimDnDBot.Core.Interfaces;

namespace SkyrimDnDBot.Application.Commands.Handlers;

public sealed class PingHandler : ICommandHandler
{
    public string Name => "ping";

    public async Task HandleAsync(IChatMessage message, ParsedCommand command)
    {
        await message.ReplyAsync("Pong!");
    }
}
