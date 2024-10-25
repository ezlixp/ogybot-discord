using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ogybot.Communication.Constants;
using ogybot.Communication.Exceptions;
using ogybot.Entities.Exceptions;
using ogybot.Utility.Extensions;

namespace ogybot.Bot.Extensions;

public static class DiscordSocketClientExtensions
{
    public static async Task RunAsync(this DiscordSocketClient client, IServiceProvider services)
    {
        var token = GetBotToken(services);

        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();

        await Task.Delay(-1);
    }

    private static string GetBotToken(IServiceProvider services)
    {
        var configuration = GetConfigurationFromServices(services);
        var token = GetTokenFromConfiguration(configuration);

        if (token.IsNullOrWhitespace())
        {
            throw new InvalidBotTokenException();
        }

        return token;
    }

    private static string? GetTokenFromConfiguration(IConfiguration configuration)
    {

        var branch = configuration.GetValue<string>("Branch");
        var token = configuration.GetValue<string>($"Tokens:{branch}");
        return token;
    }

    private static IConfiguration GetConfigurationFromServices(IServiceProvider services)
    {
        return services.GetRequiredService<IConfiguration>();
    }

    public static void AddEvents(this DiscordSocketClient client, IServiceProvider services)
    {
        client.AddLogger();
        client.AddInteraction(services);

        client.ReadyUp(services);
    }

    private static void AddLogger(this DiscordSocketClient client)
    {
        client.Log += logMessage => {
            Console.WriteLine(logMessage.Message);

            return Task.CompletedTask;
        };
    }

    private static void AddInteraction(this DiscordSocketClient client, IServiceProvider services)
    {
        client.InteractionCreated += async interaction => {
            await interaction.DeferAsync();

            await HandleCommandExecutionAsync(client, services, interaction);
        };
    }

    private static async Task HandleCommandExecutionAsync(DiscordSocketClient client, IServiceProvider services, SocketInteraction interaction)
    {

        var context = new SocketInteractionContext(client, interaction);
        var result = await TryExecuteCommandAsync(context, services);

        if (!result.IsSuccess)
        {
            await HandleCommandFailureAsync(context, result);
        }
    }

    private static async Task<IResult> TryExecuteCommandAsync(SocketInteractionContext context, IServiceProvider services)
    {
        var interactionService = services.GetRequiredService<InteractionService>();

        var result = await interactionService.ExecuteCommandAsync(context, services);

        return result;
    }

    private static async Task HandleCommandFailureAsync(SocketInteractionContext context, IResult result)
    {
        await context.Channel.SendMessageAsync(ErrorMessages.UnknownError);
        throw new UnknownException(result.ErrorReason);
    }

    private static void ReadyUp(this DiscordSocketClient client, IServiceProvider services)
    {
        client.Ready += async () => {
            var interactionService = services.GetRequiredService<InteractionService>();

            await interactionService.AddModulesAsync(Assembly.GetExecutingAssembly(), services);

            await RegisterCommandsAsync(interactionService, services);
        };
    }

    private static async Task RegisterCommandsAsync(InteractionService interactionService, IServiceProvider services)
    {
        var configuration = services.GetRequiredService<IConfiguration>();

        var id = GetGuildId(configuration);

        await interactionService.RegisterCommandsToGuildAsync(id);
    }

    private static ulong GetGuildId(IConfiguration configuration)
    {
        var branch = configuration.GetValue<string>("Branch");
        var id = configuration.GetValue<ulong>($"ServerIds:{branch}");

        return id;
    }
}