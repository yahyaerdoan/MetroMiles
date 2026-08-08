namespace MetroMiles.ApplicationLayer.Extensions.Mappings;

public static class RequiredNavigationExtensions
{
    public static T EnsureLoaded<T>(this T? value, string propertyPath) where T : class
        => value ?? throw new InvalidOperationException($"{propertyPath} was not loaded — the query must Include it.");
}
