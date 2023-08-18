using Microsoft.AspNetCore.Identity;

namespace CampusLifePlanner.Infra.Data.Identity;

public class ApplicationUser : IdentityUser
{
    private string? _FirstName;
    private string? _LastName;

    public ApplicationUser(string firstName, string lastName)
    {
        _FirstName = firstName;
        _LastName = lastName;
    }

    public string FirstName
    {
        get { return _FirstName; }
        set { _FirstName = value; }
    }

    public string LastName
    {
        get { return _LastName; }
        set { _LastName = value; }
    }

    public ApplicationUser() { }
}
