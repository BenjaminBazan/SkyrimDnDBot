using System;

namespace SkyrimDnDBot.Application.Commands;


public static class CommandParser
{
    public static ParsedCommand? TryParse (string content, char prefix)
    {
        if(string.IsNullOrWhiteSpace(content))
            return null;
        if(content[0] != prefix)
            return null;

        var rest = content.Substring(1).Trim();
        if(rest.Length == 0)
            return null;
        
        var parts = rest.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
            return null;

        var command = parts[0].ToLowerInvariant();
        var args = parts.Length > 1 ? parts[1..] : Array.Empty<string>();

        return new ParsedCommand(prefix, command, args);
    }
}

public sealed record ParsedCommand(char Prefix, string Name, string[] Args);
