using Hateoas;

namespace MetroMiles.ApplicationLayer.Features.Models.Commands.Delete;

public class DeletedModelResponse : LinkedResponse
{
    public Guid Id { get; set; }
}
