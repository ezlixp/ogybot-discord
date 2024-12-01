using Discord.Interactions;
using Discord.WebSocket;
using ogybot.Communication.Constants;
using ogybot.Communication.Exceptions;

namespace ogybot.Bot.Handlers;

public interface IBotExceptionHandler
{
    Task HandleAsync(SocketInteractionContext context, Exception exception);
}

public class BotExceptionHandler : IBotExceptionHandler
{
    public async Task HandleAsync(SocketInteractionContext context, Exception exception)
    {
        if (exception is OgybotException)
        {
            await HandleOgybotExceptionAsync(context, exception);
        }
        else
        {
            await HandleUnknownExceptionAsync(context, exception);
        }
    }

    private static async Task HandleOgybotExceptionAsync(SocketInteractionContext context, Exception exception)
    {
        await context.Interaction.FollowupAsync(exception.Message);
    }

    private static async Task HandleUnknownExceptionAsync(SocketInteractionContext context, Exception exception)
    {
        await context.Interaction.FollowupAsync(ErrorMessages.UnknownError);
        Log(exception);
    }

    private static void Log(Exception exception)
    {
        Console.WriteLine(
            "An exception occurred!" +
            $"Exception Message: {exception.Message}" +
            $"Stack Trace: {exception.StackTrace}");
    }
}