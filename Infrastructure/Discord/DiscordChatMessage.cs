using Discord.WebSocket;
using SkyrimDnDBot.Core.Interfaces;

namespace SkyrimDnDBot.Infrastructure.Discord;

public class DiscordChatMessage : IChatMessage
{
    private readonly SocketMessage _message;

    public DiscordChatMessage(SocketMessage message)
    {
        _message = message;
    }

    public string Content => _message.Content;
    public bool IsFromBot => _message.Author.IsBot;
    public string AuthorName => _message.Author.Username;

    public async Task ReplyAsync(string content)
    {
        await _message.Channel.SendMessageAsync(content);
    }
}
