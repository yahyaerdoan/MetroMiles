namespace MetroMiles.ApplicationLayer.Features.Roles.Commands.Create;

public class CreatedRoleResponse(Guid id, string name)
{
    public Guid Id { get; } = id;

    public string Name { get; } = name;
}
