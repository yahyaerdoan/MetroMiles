namespace MetroMiles.ApplicationLayer.Features.Cars.Constants;

public class CarsOperationClaims
{
    public const string Admin = "cars.admin";

    public const string Read = "cars.read";
    public const string Write = "cars.write";

    public const string Add = "cars.add";
    public const string Update = "cars.update";
    public const string Delete = "cars.delete";

    public static string[] AddRoles => [Admin, Write, Add];
    public static string[] UpdateRoles => [Admin, Write, Update];
    public static string[] DeleteRoles => [Admin, Write, Delete];
}
