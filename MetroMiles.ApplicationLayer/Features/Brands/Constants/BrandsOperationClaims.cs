namespace MetroMiles.ApplicationLayer.Features.Brands.Constants;

public class BrandsOperationClaims
{
    public const string Admin = "brands.admin";

    public const string Read = "brands.read";
    public const string Write = "brands.write";

    public const string Add = "brands.add";
    public const string Update = "brands.update";
    public const string Delete = "brands.delete";

    public static string[] AddRoles => [Admin, Write, Add];
    public static string[] UpdateRoles => [Admin, Write, Update];
    public static string[] DeleteRoles => [Admin, Write, Delete];
}
