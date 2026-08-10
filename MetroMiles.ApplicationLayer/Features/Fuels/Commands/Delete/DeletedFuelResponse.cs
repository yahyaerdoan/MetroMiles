using Hateoas;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Commands.Delete;

public class DeletedFuelResponse : LinkedResponse
{
    public Guid Id { get; set; }
}
