namespace SkyrimDnDBot.Core.Domain;

public sealed record Character(Guid Id, string Name, string Class, int Level);

