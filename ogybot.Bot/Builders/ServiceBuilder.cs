using Microsoft.Extensions.DependencyInjection;
using ogybot.Bot.Extensions;

namespace ogybot.Bot.Builders;

public static class ServiceBuilder
{
    public static IServiceProvider Build()
    {
        var services = new ServiceCollection();
        services.AddServices();

        return services.BuildServiceProvider();
    }
}