namespace MetroMiles.ApplicationLayer.Features.Cars.Constants;

public class CarMessages
{
    public const string CarNotExists = "Car does not exist";
    public const string ModelNotExists = "Model does not exist";
    public const string PlateExists = "Plate already exists";
    public const string CarNotDeleted = "Car is not deleted";
    public const string PlateAndModelCannotChangeWhileRented = "Plate and model cannot be changed while the car is rented";
    public const string CarModifiedByAnotherUser = "Car has been modified since you loaded it. Reload and try again.";
}
