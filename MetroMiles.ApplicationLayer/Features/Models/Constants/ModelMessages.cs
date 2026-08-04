namespace MetroMiles.ApplicationLayer.Features.Models.Constants;

public class ModelMessages
{
    public const string ModelNotExists = "Model does not exist";
    public const string ModelNameExists = "Model name exists";
    public const string ModelInUse = "Model is used by at least one car and cannot be deleted";
    public const string ModelNotDeleted = "Model is not deleted";
    public const string ModelModifiedByAnotherUser = "Model has been modified since you loaded it. Reload and try again.";
    public const string BrandNotExists = "Brand does not exist";
    public const string FuelNotExists = "Fuel does not exist";
    public const string TransmissionNotExists = "Transmission does not exist";
}
