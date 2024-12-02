using Discord;
using Discord.WebSocket;
using ogybot.Communication.Exceptions;
using ogybot.Domain.Security;
using ogybot.Domain.Sockets.ChatSocket;
using SocketIOClient;

namespace ogybot.Data.Sockets.Chat;

public class ChatSocketSetupHandler : IChatSocketSetupHandler
{
    private readonly SocketIOClient.SocketIO _socket;
    private readonly ITokenRequester _tokenRequester;

    public ChatSocketSetupHandler(ITokenRequester tokenRequester, SocketIOClient.SocketIO socket)
    {
        _tokenRequester = tokenRequester;
        _socket = socket;
    }

    public async Task StartAsync()
    {
        await _socket.ConnectAsync();
    }

    public async Task<IMessageChannel> GetChannelByIdAsync(DiscordSocketClient client, ulong channelId)
    {
        if (await client.GetChannelAsync(channelId) is not IMessageChannel channel)
        {
            throw new WebsocketStartupFailureException();
        }

        return channel;
    }

    public async Task RequestAndAddTokenToHeadersAsync()
    {
        var token = await _tokenRequester.GetTokenAsync();

        _socket.Options.ExtraHeaders["Authorization"] = "Bearer " + token;
    }
}