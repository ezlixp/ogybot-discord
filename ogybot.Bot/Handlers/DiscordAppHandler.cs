using System.Diagnostics.CodeAnalysis;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using ogybot.Bot.Extensions;
using ogybot.Entities.Exceptions;
using ogybot.Utility.Extensions;

namespace ogybot.Bot.Handlers;

public interface IDiscordAppHandler
{
    DiscordSocketClient SetupDiscordClient();
}

public class DiscordAppHandler(IConfiguration configuration) : IDiscordAppHandler
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