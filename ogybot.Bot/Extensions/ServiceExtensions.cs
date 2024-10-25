using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ogybot.Bot.Builders;
using ogybot.Bot.Handlers;
using ogybot.CrossCutting;

namespace ogybot.Bot.Extensions;

public static class ServiceExtensions
{
    public static void AddServices(this ServiceCollection services)
    {
        services.AddConfiguration();
        services.AddHandlers();
        services.AddDiscordClient();
        services.AddInteractionService();

        // Dependencies from other projects, such as Infrastructure
        services.AddDependencies();
    }

    private static void AddConfiguration(this ServiceCollection services)
    {
        services.AddSingleton(AppConfigurationBuilder.Build());
    }

    private static void AddHandlers(this ServiceCollection services)
    {
        services.AddSingleton<IDiscordAppHandler, DiscordAppHandler>();
    }

    private static void AddDiscordClient(this ServiceCollection services)
    {
        services.AddSingleton<DiscordSocketClient>(provider => {
            var handler = provider.GetRequiredService<IDiscordAppHandler>();

            return handler.SetupDiscordClient();
        });
    }

    private static void AddInteractionService(this ServiceCollection services)
    {
        services.AddSingleton<InteractionService>(provider => {
            var client = provider.GetRequiredService<DiscordSocketClient>();

            return new InteractionService(client.Rest);
        });
    }
}