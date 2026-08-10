namespace MetroMiles.ApplicationLayer.Features.Cars.Common;

public static class MileageConverter
{
    private const double KilometersPerMile = 1.60934;

    public static int ResolveKilometer(int? kilometer, int? mile) =>
        kilometer ?? (int)Math.Round(mile!.Value * KilometersPerMile);

    public static int ResolveMile(int? kilometer, int? mile) =>
        mile ?? (int)Math.Round(kilometer!.Value / KilometersPerMile);
}
