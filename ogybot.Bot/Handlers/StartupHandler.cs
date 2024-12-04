using Discord.WebSocket;
using ogybot.Bot.Extensions;
using ogybot.Communication.Constants;
using ogybot.Domain.Sockets.ChatSocket;

namespace ogybot.Bot.Handlers;

public interface IStartupHandler
{
    Task StartupComponentsAsync(IServiceProvider services);
}

public class StartupHandler : IStartupHandler
{
    private readonly IChatSocket _chatSocket;
    private readonly DiscordSocketClient _client;

    public StartupHandler(IChatSocket chatSocket, DiscordSocketClient client)
    {
        _chatSocket = chatSocket;
        _client = client;
    }

    public async Task StartupComponentsAsync(IServiceProvider services)
    {
        // The discord bot service must be started first. Chat socket relies on it to function.
        await StartupDiscordBotAsync(services);
        await StartupChatSocketAsync();
    }

    private async Task StartupDiscordBotAsync(IServiceProvider services)
    {
        _client.AddEvents(services);
        await _client.RunAsync(services);
    }

    private async Task StartupChatSocketAsync()
    {
        await _chatSocket.SetupAndStartAsync(_client, GuildChannels.WebsocketLogChannel);
    }
}