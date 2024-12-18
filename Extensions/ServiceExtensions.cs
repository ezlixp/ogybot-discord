﻿using System.Net;
using System.Net.WebSockets;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ogybot.Builders;
using ogybot.DataAccess.Clients;
using ogybot.DataAccess.Controllers;
using ogybot.DataAccess.Security;
using ogybot.DataAccess.Sockets;

namespace ogybot.Extensions;

public static class ServiceExtensions
{
    public static void AddServices(this ServiceCollection services)
    {
        services.AddConfiguration();
        services.AddDiscordClient();
        services.AddInteractionService();
        services.AddHttpClient();
        services.AddTokenGenerator();
        services.AddClients();
        services.AddControllers();
        services.AddSockets();
    }

    private static void AddConfiguration(this ServiceCollection services)
    {
        services.AddSingleton(AppConfigurationBuilder.Build());
    }

    private static void AddDiscordClient(this ServiceCollection services)
    {
        services.AddSingleton<DiscordSocketClient>(provider => {
            var config = provider.GetRequiredService<IConfiguration>();

            var discordClient = DiscordAppBuilder.SetupDiscordClientAsync(config).GetAwaiter().GetResult();

            return discordClient;
        });
    }

    private static void AddInteractionService(this ServiceCollection services)
    {
        services.AddSingleton<InteractionService>(provider => {
            var client = provider.GetRequiredService<DiscordSocketClient>();

            return new InteractionService(client.Rest);
        });
    }

    private static void AddHttpClient(this ServiceCollection services)
    {
        services.AddSingleton<HttpClient>(provider => {
            var config = provider.GetRequiredService<IConfiguration>();
            var baseAddress = config["Api:Uri"]!;

            return new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
        });
    }

    private static void AddClients(this ServiceCollection services)
    {
        services.AddSingleton<TomeClient>();
        services.AddSingleton<WaitlistClient>();
        services.AddSingleton<AspectClient>();
        services.AddSingleton<OnlinePlayersClient>();
    }

    private static void AddControllers(this ServiceCollection services)
    {
        services.AddSingleton<TomelistController>();
        services.AddSingleton<WaitlistController>();
        services.AddSingleton<AspectsController>();
        services.AddSingleton<OnlinePlayersController>();
    }

    private static void AddSockets(this ServiceCollection services)
    {
        services.AddSingleton<ChatSocket>(provider => {
            var tokenGenerator = provider.GetRequiredService<TokenGenerator>();
            var config = provider.GetRequiredService<IConfiguration>();
            var webSocketUrl = config["Websocket:WebsocketServerUrl"]!;

            return new ChatSocket(tokenGenerator, webSocketUrl);
        });
    }

    private static void AddTokenGenerator(this ServiceCollection services)
    {
        services.AddSingleton<TokenGenerator>(provider => {
            var config = provider.GetRequiredService<IConfiguration>();
            var validationKey = config["Api:ValidationKey"]!;

            var httpClient = provider.GetRequiredService<HttpClient>();

            return new TokenGenerator(httpClient, validationKey);
        });
    }
}