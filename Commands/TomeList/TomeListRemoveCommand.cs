﻿using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
using ogybot.DataAccess.Controllers;
using ogybot.DataAccess.Entities;

namespace ogybot.Commands.TomeList;

public abstract class TomeListRemoveCommand : ICommand
{
    private static readonly TomelistController Controller = new();

    public static async Task ExecuteCommandAsync(SocketSlashCommand command)
    {
        var input = command.Data.Options.FirstOrDefault()!.Value.ToString();

        if (input!.Contains(' ') && !input.Contains(','))
        {
            await command.FollowupAsync("You cannot submit usernames with whitespaces");
            return;
        }

        var inputList = input.Split(',')
            .Select(user => user.Trim())
            .Distinct()
            .ToList();

        var responseList = new List<Response>();

        foreach (var singleInput in inputList)
        {
            // Check if the input can be converted to an integer. if so, remove user in that position in the list.
            if (int.TryParse(singleInput, out var index))
            {
                await RemoveByIndex(index, responseList);
                continue;
            }

            // In this case, the input is the user's name
            await RemoveByName(singleInput, responseList);
        }

        var statusList = responseList
            .Select(response => response.Status);

        var errorList = responseList
            .Select(response => response.Error)
            .Where(error => error is not null)
            .Distinct();

        if (statusList.Contains(false))
        {
            var formattedErrorList = errorList.Aggregate("", (current, error) => current + $"'{error}'" + ", ");

            await command.FollowupAsync($"One or multiple errors occurred: {formattedErrorList[..^2]}");
            return;
        }

        var users = responseList
            .Select(user => user.Username)
            .Aggregate("", (current, user) => current + ($"'{user}'" + ", "));

        // [..^n] removes the last n characters of an array
        var msg = $"Successfully removed players {users[..^2]} from the tome list.";

        await command.FollowupAsync(msg);
    }

    private static async Task RemoveByName(string username, List<Response> responseList)
    {
        var result = await Controller.RemovePlayerAsync(new UserTomelist { Username = username });

        responseList.Add(result);
    }

    private static async Task RemoveByIndex(int index, List<Response> responseList)
    {
        var list = await Controller.GetTomelistAsync();
        var username = list[index - 1].Username;

        var result = await Controller.RemovePlayerAsync(new UserTomelist { Username = username });

        responseList.Add(result);
    }

    public static async Task GenerateCommandAsync(DiscordSocketClient socketClient, ulong guildId)
    {
        try
        {
            var guildCommand = new SlashCommandBuilder()
                .WithName("tomelist-remove")
                .WithDescription("Removes user from tome list")
                .AddOption("username", ApplicationCommandOptionType.String, "User you're removing", true);
            await socketClient.Rest.CreateGuildCommand(guildCommand.Build(), guildId);
        }
        catch (HttpException exception)
        {
            var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

            Console.WriteLine(json);
        }
    }
}