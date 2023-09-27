namespace CampusLifePlanner.Domain.Account;

public interface IAuthenticate
{
    Task<bool> UpdateUserProfile(string userId, string imgPath);
    Task<bool> Authentication(string email, string password, bool rememberMe); 
    Task<(bool success, string msg)> RegisterUser(string firstName, string lastName, string email, string password);
    Task<bool> EmailExists(string email);
    Task Logout();
}
