using Hateoas;

namespace MetroMiles.ApplicationLayer.Features.Models.Commands.Restore;

public class RestoredModelResponse : LinkedResponse
{
    public Guid Id { get; set; }
}
