namespace ogybot.Domain.Entities;

public abstract class BaseUser
{

    public BaseUser()
    {

    }

    public BaseUser(string username)
    {
        Username = username;
    }

    public string? Username { get; set; }
}