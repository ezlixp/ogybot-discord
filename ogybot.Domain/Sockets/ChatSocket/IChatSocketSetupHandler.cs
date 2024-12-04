using Discord;
using Discord.WebSocket;

namespace ogybot.Domain.Sockets.ChatSocket;

public interface IChatSocketSetupHandler
{
    Task StartAsync();
    Task<IMessageChannel> GetChannelByIdAsync(DiscordSocketClient client, ulong channelId);
    Task RequestAndAddTokenToHeadersAsync();
    Task RequestAndRefreshTokenInHeadersAsync();
}