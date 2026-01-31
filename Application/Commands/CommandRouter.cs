

using System;
using Discord.WebSocket;

namespace SkyrimDnDBot.Application.Commands;

public sealed class CommandRouter
{
    private readonly char _prefix;

    public CommandRouter(char prefix)
    {
        _prefix = prefix;
    }

    public async Task RouteAsync(SocketMessage message)
    {
        if (message.Author.IsBot)
            return;
        
        var parsedCommand = CommandParser.TryParse(message.Content, _prefix);
        
        if (parsedCommand is null)
            return;
        
        switch (parsedCommand.Name)
        {
            case "que":
                await message.Channel.SendMessageAsync("so! JAJAJAJ (?)");
                break;
            case "hello":
                await message.Channel.SendMessageAsync($"Kuibo prro, wenas {message.Author.Mention}!");
                break;
            case "help" :
                var helpText = GetHelpText();
                await message.Channel.SendMessageAsync(helpText);
                break;     
            default:
                await message.Channel.SendMessageAsync($"Aun no hice este comando, da paja: {parsedCommand.Name}");
                break;

        }
    }

    private string GetHelpText()
    {
        return $"Available commands:\n" +
               $"{_prefix}que - no se sabe (? )'\n" +
               $"{_prefix}hello - Saludiño \n" +
               $"{_prefix}help - Muestra un menu con todo";
    }

}
