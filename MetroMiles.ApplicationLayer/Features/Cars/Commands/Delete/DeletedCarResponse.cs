using Hateoas;

namespace MetroMiles.ApplicationLayer.Features.Cars.Commands.Delete;

public class DeletedCarResponse : LinkedResponse
{
    public Guid Id { get; set; }
}
