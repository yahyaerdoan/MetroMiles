namespace MetroMiles.ApplicationLayer.Features.Brands.Constants;

public class BrandMessages
{
    public const string BrandNameExists = "Brand name exists";
    public const string BrandNotExists = "Brand does not exist";
    public const string BrandInUse = "Brand is used by at least one model and cannot be deleted";
    public const string BrandNotDeleted = "Brand is not deleted";
    public const string BrandModifiedByAnotherUser = "Brand has been modified since you loaded it. Reload and try again.";
}
