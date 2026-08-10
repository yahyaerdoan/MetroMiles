using Hateoas;

namespace MetroMiles.ApplicationLayer.Features.Brands.Commands.Delete;

public class DeletedBrandResponse : LinkedResponse
{
    public Guid Id { get; set; }
}
