namespace MetroMiles.ApplicationLayer.Features.Users.Constants;

public class UsersOperationClaims
{
    public const string Admin = "users.admin";

    public const string Read = "users.read";
    public const string Write = "users.write";

    public const string Add = "users.add";
    public const string Update = "users.update";
    public const string Delete = "users.delete";

    public static string[] AddRoles => [Admin, Write, Add];
    public static string[] UpdateRoles => [Admin, Write, Update];
    public static string[] DeleteRoles => [Admin, Write, Delete];
}
