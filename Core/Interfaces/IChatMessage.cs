namespace SkyrimDnDBot.Core.Interfaces;

public interface IChatMessage
{
    string Content { get; }
    bool IsFromBot { get; }
    string AuthorName { get; }
    Task ReplyAsync(string content);
}
