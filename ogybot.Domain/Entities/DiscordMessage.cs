namespace ogybot.Domain.Entities;

public class DiscordMessage
{
    public string Author { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public DiscordMessage()
    {
    }

    public DiscordMessage(string author, string content)
    {
        Author = author;
        Content = content;
    }
}