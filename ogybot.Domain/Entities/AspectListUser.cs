namespace ogybot.Domain.Entities;

public class AspectListUser : BaseUser
{
    public double Aspects { get; set; }

    public AspectListUser()
    {

    }

    public AspectListUser(string username) : base(username)
    {

    }
}