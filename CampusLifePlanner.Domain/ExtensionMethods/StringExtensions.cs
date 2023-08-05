namespace CampusLifePlanner.Domain.ExtensionMethods;

public static class StringExtensions
{
    public static void EnsureNotNullOrEmpty(this string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{paramName} cannot be null or whitespace", paramName);
        }
    }
}
