namespace ogybot.Domain.Entities;

public abstract class BaseUser
{
    public string? Username { get; set; }

    public BaseUser()
    {

    }

    public BaseUser(string username)
    {
        Username = username;
    }
}