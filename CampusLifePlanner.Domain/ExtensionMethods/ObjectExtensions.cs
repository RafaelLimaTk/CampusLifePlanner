namespace CampusLifePlanner.Domain.ExtensionMethods;

public static class ObjectExtensions
{
    public static void EnsureNotNull<T>(this T obj, string parameterName)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(parameterName);
        }
    }
}
