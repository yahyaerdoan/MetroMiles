namespace MetroMiles.ApplicationLayer.Features.Transmissions.Constants;

public class TransmissionMessages
{
    public const string TransmissionNotExists = "Transmission does not exist";
    public const string TransmissionNameExists = "Transmission name exists";
    public const string TransmissionInUse = "Transmission is used by at least one model and cannot be deleted";
    public const string TransmissionNotDeleted = "Transmission is not deleted";
    public const string TransmissionModifiedByAnotherUser = "Transmission has been modified since you loaded it. Reload and try again.";
}
