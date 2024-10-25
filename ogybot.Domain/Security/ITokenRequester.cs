namespace ogybot.Domain.Security;

public interface ITokenRequester
{
    Task<string> GetTokenAsync();
}