using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroMiles.ApplicationLayer.Features.Users.Constants;

public class UsersOperationClaims
{
    public const string Admin = "users.admin";

    public const string Read = "users.read";
    public const string Write = "users.write";

    public const string Add = "users.add";
    public const string Update = "users.update";
    public const string Delete = "users.delete";
}
