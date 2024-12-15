using System;

namespace WebApiAutores.DTOs;

public class DataHATEOAS(string? link, string description, string method)
{
    public string? Link { get; private set; } = link;
    public string Description { get; private set; } = description;
    public string Method { get; private set; } = method;
}
