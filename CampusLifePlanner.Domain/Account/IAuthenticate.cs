namespace CampusLifePlanner.Domain.Account;

public interface IAuthenticate
{
    Task<bool> Authentication(string email, string password);
    Task<(bool success, string msg)> RegisterUser(string firstName, string lastName, string email, string password);
    Task Logout();
}
