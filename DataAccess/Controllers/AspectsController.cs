﻿using test.DataAccess.Clients;
using test.DataAccess.Entities;

namespace test.DataAccess.Controllers;

/// <summary>
/// Class responsible for handling aspect-related command requests
/// </summary>
public class AspectsController
{
    private readonly AspectClient _aspectClient = new AspectClient();

    public async Task<IEnumerable<UserAspectlist>?> GetAspectListAsync()
    {
        // Makes an HTTP request to get list,
        // then sorts its members by aspect
        // count.
        var list = await _aspectClient.GetAspectsOwedListAsync();
        
        return list?.OrderByDescending(user => user.Aspects);
    }

    public async Task<Response> DecrementPlayersAspectsAsync(IEnumerable<string> players)
    {
        return await _aspectClient.DecrementAspectFromPlayerAsync(players);
    }
}