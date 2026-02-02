using SkyrimDnDBot.Application.Commands;
using SkyrimDnDBot.Core.Interfaces;

namespace SkyrimDnDBot.Application.Commands.Interfaces;

public interface ICommandHandler
{
    string Name { get; }
    Task HandleAsync(IChatMessage message, ParsedCommand command);
}
