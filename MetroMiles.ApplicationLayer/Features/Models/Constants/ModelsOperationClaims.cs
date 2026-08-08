namespace MetroMiles.ApplicationLayer.Features.Models.Constants;

public class ModelsOperationClaims
{
    public const string Admin = "models.admin";

    public const string Read = "models.read";
    public const string Write = "models.write";

    public const string Add = "models.add";
    public const string Update = "models.update";
    public const string Delete = "models.delete";

    public static string[] AddRoles => [Admin, Write, Add];
    public static string[] UpdateRoles => [Admin, Write, Update];
    public static string[] DeleteRoles => [Admin, Write, Delete];
}
