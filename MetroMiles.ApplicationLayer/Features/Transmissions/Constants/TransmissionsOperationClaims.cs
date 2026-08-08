namespace MetroMiles.ApplicationLayer.Features.Transmissions.Constants;

public class TransmissionsOperationClaims
{
    public const string Admin = "transmissions.admin";

    public const string Read = "transmissions.read";
    public const string Write = "transmissions.write";

    public const string Add = "transmissions.add";
    public const string Update = "transmissions.update";
    public const string Delete = "transmissions.delete";

    public static string[] AddRoles => [Admin, Write, Add];
    public static string[] UpdateRoles => [Admin, Write, Update];
    public static string[] DeleteRoles => [Admin, Write, Delete];
}
