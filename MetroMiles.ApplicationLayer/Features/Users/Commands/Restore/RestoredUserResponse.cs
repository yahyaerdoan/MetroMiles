using Hateoas;

namespace MetroMiles.ApplicationLayer.Features.Users.Commands.Restore;

public class RestoredUserResponse : LinkedResponse
{
    public Guid Id { get; set; }
}
