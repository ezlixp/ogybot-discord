using Discord.Interactions;
using ogybot.Bot.Handlers;
using ogybot.Communication.Constants;

namespace ogybot.Bot.Commands.Base;

public abstract class BaseCommand : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IBotExceptionHandler _botExceptionHandler;

    protected BaseCommand(IBotExceptionHandler exceptionHandler)
    {
        _botExceptionHandler = exceptionHandler;
    }

    protected async Task<bool> IsInvalidChannelAsync(ulong channelId)
    {
        if (Context.Channel.Id == channelId) return false;

        await FollowupAsync(ErrorMessages.InvalidChannelError);
        return true;
    }

    protected async Task TryExecutingCommandInstructionsAsync(Func<Task> command)
    {
        try
        {
            await command();
        }
        catch (Exception e)
        {
            await _botExceptionHandler.HandleAsync(Context, e);
        }
    }
}