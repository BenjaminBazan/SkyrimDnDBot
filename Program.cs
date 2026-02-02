// See https://aka.ms/new-console-template for more information
using Discord;
using Discord.WebSocket;
using SkyrimDnDBot.Application.Commands;
using SkyrimDnDBot.Application.Commands.Handlers;
using SkyrimDnDBot.Application.Commands.Interfaces;
using SkyrimDnDBot.Infrastructure.Discord;

class Program
{
    private DiscordSocketClient _client;
    private CommandRouter _commandRouter;

    static Task Main(string[] args) => new Program().MainAsync();

    public async Task MainAsync()
    {
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages | GatewayIntents.MessageContent
        });
        var prefix = '%';
        var handlers = new ICommandHandler[]
        {
            new PingHandler(),
            new HelpHandler(prefix)
        };
        _commandRouter = new CommandRouter(prefix, handlers);
        _client.Log += Log;
        _client.MessageReceived += MessageReceived;

        var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
     
        Console.WriteLine("Hoooola funcionando ando jeje");
        await Task.Delay(-1);

    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private async Task MessageReceived(SocketMessage message)
    {
        var chatMessage = new DiscordChatMessage(message);
        await _commandRouter.RouteAsync(chatMessage);
    }
}
