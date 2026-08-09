using Core.SecurityLayer.Identity;

namespace MetroMiles.DomainLayer.Entities;

public class Role : SequentialGuidIdentityRole
{
    public Role() { }

    public Role(string roleName) : base(roleName) { }
}
