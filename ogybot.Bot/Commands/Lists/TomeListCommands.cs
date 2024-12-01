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

public class TomeListCommands : BasePermissionRequiredCommand
{
    private readonly ITomeListClient _tomeListClient;
    private readonly IListCommandValidator _commandValidator;

    public TomeListCommands(
        ITomeListClient tomeListClient,
        IListCommandValidator commandValidator,
        IBotExceptionHandler exceptionHandler) : base(exceptionHandler)
    {
        _tomeListClient = tomeListClient;
        _commandValidator = commandValidator;
    }

    #region List Command

    [CommandContextType(InteractionContextType.Guild)]
    [SlashCommand("tomelist", "Presents the tome list to get a guild tome.")]
    public async Task ExecuteTomeListCommandAsync()
    {
        if (await IsInvalidChannelAsync(GuildChannels.TomeChannel)) return;

        await TryExecutingCommandInstructionsAsync(TomeListCommandInstructionsAsync);
    }

    private async Task TomeListCommandInstructionsAsync()
    {
        var embed = await CreateEmbedAsync();

        await FollowupAsync(embed: embed);
    }

    private async Task<Embed> CreateEmbedAsync()
    {
        var content = await GetEmbedContentAsync();

        var embedBuilder = new EmbedBuilder()
            .WithAuthor(content.SocketUser.Username, content.SocketUser.GetAvatarUrl() ?? content.SocketUser.GetDefaultAvatarUrl())
            .WithTitle("Tome List")
            .WithDescription(content.Description)
            .WithColor(Color.Teal)
            .WithCurrentTimestamp()
            .WithFooter(content.QueueSize);

        return embedBuilder.Build();
    }

    private async Task<EmbedContent> GetEmbedContentAsync()
    {
        var list = await _tomeListClient.GetListAsync();

        var user = Context.User;
        var queueSize = "Players in queue: " + list.Count;
        var description = CreateEmbedDescription(list);

        return new EmbedContent(user, queueSize, description);
    }

    private static string CreateEmbedDescription(IList<TomeListUser> list)
    {
        var description = "";

        var counter = 1;

        foreach (var tomeListUser in list)
        {
            description += $"{counter}. {tomeListUser.Username}\n";

            counter++;
        }

        return description;
    }

    #endregion

    #region Add To List Command

    [CommandContextType(InteractionContextType.Guild)]
    [SlashCommand("tomelist-add", "Adds a user to the tome list.")]
    public async Task ExecuteTomeListAddCommandAsync([Summary("user", "User to insert into the tome list")] string username)
    {
        if (await IsInvalidChannelAsync(GuildChannels.TomeChannel))
        {
            return;
        }

        await TryExecutingCommandInstructionsAsync(async () => await TomeListAddCommandInstructionsAsync(username));
    }

    private async Task TomeListAddCommandInstructionsAsync(string username)
    {
        await ValidateUsernameAsync(username);

        await AddUserToTomeListAsync(username);

        await FollowupAsync($"Successfully added player {username} to the wait list.");
    }

    private async Task AddUserToTomeListAsync(string username)
    {
        var tomeListUser = new TomeListUser(username);

        await _tomeListClient.AddUserAsync(tomeListUser);
    }

    private async Task ValidateUsernameAsync(string username)
    {
        var userList = await _tomeListClient.GetListAsync();
        _commandValidator.ValidateUsername(userList, username);
    }

    #endregion

    #region Remove User From List Command

    [CommandContextType(InteractionContextType.Guild)]
    [SlashCommand("tomelist-remove", "removes a user from the tome list based on their name or index")]
    public async Task ExecuteTomeListRemoveCommandAsync([Summary("users-or-indexes", "The user's name or index")] string usernamesOrIndexes)
    {
        if (await IsInvalidContextAsync(GuildChannels.TomeChannel)) return;

        await TryExecutingCommandInstructionsAsync(async () => await TomeListRemoveInstructionsAsync(usernamesOrIndexes));
    }

    private async Task TomeListRemoveInstructionsAsync(string usernamesOrIndexes)
    {
        if (usernamesOrIndexes.Contains(','))
        {
            await RemoveMultiplePlayersFromListAsync(usernamesOrIndexes);
        }
        else
        {
            await RemovePlayerFromListAsync(usernamesOrIndexes);
        }

        await FollowupAsync("Successfully removed provided player(s) from the wait list.");
    }

    private async Task RemoveMultiplePlayersFromListAsync(string usernamesOrIndexes)
    {
        var players = usernamesOrIndexes
            .Split(',')
            .Select(player => player.Trim())
            .Where(player => !player.IsNullOrWhitespace())
            .OrderDescending();

        foreach (var player in players)
        {
            await RemovePlayerFromListAsync(player);
        }
    }

    private async Task RemovePlayerFromListAsync(string usernameOrIndex)
    {
        if (short.TryParse(usernameOrIndex, out var index))
        {
            await RemoveByIndexAsync(index);
        }
        else
        {
            await RemoveByNameAsync(usernameOrIndex);
        }
    }

    private async Task RemoveByIndexAsync(int index)
    {
        var list = await _tomeListClient.GetListAsync();

        ValidateUserBeingRemovedByIndex(index, list);

        // Gets the user based on the index provided. As the list count starts at 1, the index has to be subtracted by 1.
        var tomeListUser = list[index - 1];

        await _tomeListClient.RemoveUserAsync(tomeListUser);
    }

    private async Task RemoveByNameAsync(string username)
    {
        await ValidateUserBeingRemovedByName(username);

        var tomeListUser = new TomeListUser(username);

        await _tomeListClient.RemoveUserAsync(tomeListUser);
    }

    private async Task ValidateUserBeingRemovedByName(string username)
    {
        var list = await _tomeListClient.GetListAsync();

        _commandValidator.ValidateUserRemoval(list, username);
    }

    private void ValidateUserBeingRemovedByIndex(int index, IList<TomeListUser> list)
    {
        _commandValidator.ValidateUserRemoval(list, index);
    }

    #endregion

}