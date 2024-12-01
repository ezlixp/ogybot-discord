using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ogybot.Bot.Builders;
using ogybot.Bot.Extensions;
using ogybot.Bot.Handlers;
using ogybot.Communication.Constants;
using ogybot.Domain.Sockets;

namespace ogybot.Bot;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var services = ServiceBuilder.Build();
        var startupHandler = services.GetRequiredService<IStartupHandler>();

        await startupHandler.StartupComponentsAsync(services);
        
        await Task.Delay(Timeout.Infinite);
    }
}