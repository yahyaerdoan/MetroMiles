using Hateoas;

namespace MetroMiles.ApplicationLayer.Features.Users.Commands.Delete;

public class DeletedUserResponse : LinkedResponse
{
    public Guid Id { get; set; }
}
