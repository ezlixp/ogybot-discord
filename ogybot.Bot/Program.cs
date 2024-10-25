using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ogybot.Bot.Builders;
using ogybot.Bot.Extensions;

namespace ogybot.Bot;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var services = ServiceBuilder.Build();
        var discordClient = services.GetRequiredService<DiscordSocketClient>();

        discordClient.AddEvents(services);

        await discordClient.RunAsync(services);
    }
}