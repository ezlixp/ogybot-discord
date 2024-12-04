using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace ogybot.Bot.Handlers;

public interface IDiscordAppHandler
{
    DiscordSocketClient SetupDiscordClient();
}

public class DiscordAppHandler : IDiscordAppHandler
{
    public DiscordSocketClient SetupDiscordClient()
    {
        var botConfig = new DiscordSocketConfig
        {
            UseInteractionSnowflakeDate = false,
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };

        var discordClient = new DiscordSocketClient(botConfig);

        return discordClient;
    }
}