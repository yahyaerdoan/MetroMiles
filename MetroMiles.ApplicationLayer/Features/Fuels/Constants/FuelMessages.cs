namespace MetroMiles.ApplicationLayer.Features.Fuels.Constants;

public class FuelMessages
{
    public const string FuelNotExists = "Fuel does not exist";
    public const string FuelNameExists = "Fuel name exists";
    public const string FuelInUse = "Fuel is used by at least one model and cannot be deleted";
    public const string FuelNotDeleted = "Fuel is not deleted";
    public const string FuelModifiedByAnotherUser = "Fuel has been modified since you loaded it. Reload and try again.";
}
