using Discord;
using Discord.Interactions;
using ogybot.Bot.Commands.Base;
using ogybot.Bot.Commands.Lists.Validators;
using ogybot.Bot.Handlers;
using ogybot.Communication.Constants;
using ogybot.Domain.Clients;
using ogybot.Domain.Entities;
using ogybot.Utility.Extensions;

namespace ogybot.Bot.Commands.Lists;

public class AspectListCommands : BasePermissionRequiredCommand
{

    private readonly IAspectListClient _aspectListClient;
    private readonly IListCommandValidator _commandValidator;

    private const ulong ChannelId = GuildChannels.RaidsChannel;

    public AspectListCommands(
        IAspectListClient aspectListClient,
        IBotExceptionHandler exceptionHandler,
        IListCommandValidator commandValidator) : base(exceptionHandler)
    {
        _aspectListClient = aspectListClient;
        _commandValidator = commandValidator;
    }

    #region List Command

    [CommandContextType(InteractionContextType.Guild)]
    [SlashCommand("aspectlist", "Presents the aspect list to get a guild aspect.")]
    public async Task ExecuteAspectListCommandAsync()
    {
        if (await IsInvalidChannelAsync(ChannelId))
        {
            return;
        }

        await TryExecutingCommandInstructionsAsync(AspectListCommandInstructionsAsync);
    }

    private async Task AspectListCommandInstructionsAsync()
    {

        var embed = await CreateEmbedAsync();

        await FollowupAsync(embed: embed);
    }

    private async Task<Embed> CreateEmbedAsync()
    {
        // Create class to store this info later
        var content = await GetEmbedContentAsync();

        var embedBuilder = new EmbedBuilder()
            .WithAuthor(content.SocketUser.Username, content.SocketUser.GetAvatarUrl() ?? content.SocketUser.GetDefaultAvatarUrl())
            .WithTitle("Aspect List")
            .WithDescription(content.Description)
            .WithColor(Color.Teal)
            .WithCurrentTimestamp()
            .WithFooter(content.QueueSize);

        return embedBuilder.Build();
    }

    private async Task<EmbedContent> GetEmbedContentAsync()
    {
        var list = await _aspectListClient.GetListAsync();

        var user = Context.User;
        var queueSize = "Players in queue: " + list.Count;
        var description = CreateEmbedDescription(list);

        return new EmbedContent(user, queueSize, description);
    }

    private static string CreateEmbedDescription(IList<AspectListUser> list)
    {
        var description = "";

        var counter = 1;

        foreach (var aspectListUser in list)
        {
            description += $"{counter}. {aspectListUser.Username}: {aspectListUser.Aspects}\n";

            counter++;
        }

        return description;
    }

    #endregion

    #region Decrement Aspect Command

    [CommandContextType(InteractionContextType.Guild)]
    [SlashCommand("aspectlist-decrement", "Decrements an aspect from the provided user.")]
    public async Task ExecuteAspectListDecrementCommandAsync([Summary("users-or-indexes", "The user's name or index")] string usernamesOrIndexes)
    {
        if (await IsInvalidContextAsync(ChannelId))
        {
            return;
        }

        await TryExecutingCommandInstructionsAsync(async () => await AspectListDecrementCommandInstructionsAsync(usernamesOrIndexes));
    }

    private async Task AspectListDecrementCommandInstructionsAsync(string usernamesOrIndexes)
    {
        if (usernamesOrIndexes.Contains(','))
        {
            await DecrementAspectFromMultiplePlayersAsync(usernamesOrIndexes);
        }
        else
        {
            await DecrementAspectFromPlayerAsync(usernamesOrIndexes);
        }

        await FollowupAsync("Successfully decremented 1 aspect from the provided player(s).");
    }

    private async Task DecrementAspectFromMultiplePlayersAsync(string usernamesOrIndexes)
    {
        var players = usernamesOrIndexes
            .Split(',')
            .Select(player => player.Trim())
            .Where(player => !player.IsNullOrWhitespace())
            .OrderDescending();

        foreach (var player in players)
        {
            await DecrementAspectFromPlayerAsync(player);
        }
    }

    private async Task DecrementAspectFromPlayerAsync(string usernameOrIndex)
    {
        if (short.TryParse(usernameOrIndex, out var index))
        {
            await DecrementByIndexAsync(index);
        }
        else
        {
            await DecrementByNameAsync(usernameOrIndex);
        }
    }

    private async Task DecrementByNameAsync(string username)
    {
        var list = await _aspectListClient.GetListAsync();

        _commandValidator.ValidateUserRemoval(list, username);

        var aspectListUser = new AspectListUser(username);

        await _aspectListClient.DecrementAspectAsync(aspectListUser);
    }

    private async Task DecrementByIndexAsync(int index)
    {
        var list = await _aspectListClient.GetListAsync();

        _commandValidator.ValidateUserRemoval(list, index);

        // Gets the user based on the index provided. As the list count starts at 1, the index has to be subtracted by 1.
        var aspectListUser = list[index - 1];

        await _aspectListClient.DecrementAspectAsync(aspectListUser);
    }

    #endregion

}