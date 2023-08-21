using Microsoft.AspNetCore.Identity;

namespace CampusLifePlanner.Infra.Data.Identity;

public class ApplicationUser : IdentityUser
{
    private string? _FirstName;
    private string? _LastName;
    private string? _ImgPath;

    public ApplicationUser(string firstName, string lastName, string? imgPath)
    {
        _FirstName = firstName;
        _LastName = lastName;
        _ImgPath = imgPath;
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

    public string ImgPath
    {
        get { return _ImgPath; }
        set { _ImgPath = value; }
    }

    public ApplicationUser() { }
}
