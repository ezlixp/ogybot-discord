using Discord;
using Discord.WebSocket;
using ogybot.Domain.Sockets.ChatSocket;

namespace ogybot.Data.Sockets.Chat;

public class ChatSocket : IChatSocket
{
    private readonly IChatSocketCommunicationHandler _communicationHandler;
    private readonly IChatSocketSetupHandler _setupHandler;

    public ChatSocket(IChatSocketSetupHandler setupHandler, IChatSocketCommunicationHandler communicationHandler)
    {
        _setupHandler = setupHandler;
        _communicationHandler = communicationHandler;
    }

    public async Task SetupAndStartAsync(DiscordSocketClient client, ulong channelId)
    {
        var channel = await _setupHandler.GetChannelByIdAsync(client, channelId);

        await SetupClientAsync(channel);
        _communicationHandler.SetupEmitter(client, channel);
        await _setupHandler.StartAsync();
    }

    public async Task EmitMessageAsync(SocketUserMessage message)
    {
        await _communicationHandler.EmitMessageAsync(message);
    }

    private async Task SetupClientAsync(IMessageChannel channel)
    {
        await _setupHandler.RequestAndAddTokenToHeadersAsync();
        _communicationHandler.SetupEventListeners(channel);
    }
}