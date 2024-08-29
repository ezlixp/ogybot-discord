﻿namespace test.Api.Entities;

public record AspectApiResponse
{
    public bool Status { get; init; }
    public string? Token { get; init; }
    public string? Error { get; init; }
}