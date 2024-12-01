using Discord.WebSocket;

namespace ogybot.Domain.Sockets.ChatSocket;

public interface IChatSocket
{
    Task SetupAndStartAsync(DiscordSocketClient client, ulong channelId);
    Task EmitMessageAsync(SocketUserMessage message);
}