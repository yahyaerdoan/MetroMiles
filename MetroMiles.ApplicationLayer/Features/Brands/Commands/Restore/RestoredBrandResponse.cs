using Hateoas;

namespace MetroMiles.ApplicationLayer.Features.Brands.Commands.Restore;

public class RestoredBrandResponse : LinkedResponse
{
    public Guid Id { get; set; }
}
