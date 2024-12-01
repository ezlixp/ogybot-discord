using Discord;
using Discord.WebSocket;
using ogybot.Communication.Exceptions;
using ogybot.Domain.Security;
using ogybot.Domain.Sockets;
using ogybot.Domain.Sockets.ChatSocket;
using SocketIOClient;

namespace ogybot.Data.Sockets.Chat;

public class ChatSocket : IChatSocket
{
    private readonly IChatSocketSetupHandler _setupHandler;
    private readonly IChatSocketCommunicationHandler _communicationHandler;

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

    private async Task SetupClientAsync(IMessageChannel channel)
    {
        await _setupHandler.RequestAndAddTokenToHeadersAsync();
        _communicationHandler.SetupEventListeners(channel);
    }

    public async Task EmitMessageAsync(SocketUserMessage message)
    {
        await _communicationHandler.EmitMessageAsync(message);
    }
}