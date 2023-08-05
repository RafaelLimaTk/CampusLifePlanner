namespace CampusLifePlanner.Domain.Account;

public interface IAuthenticate
{
    Task<bool> Authentication(string email, string password);
    Task<bool> RegisterUser(string email, string password);
    Task Logout();
}
