using Core.PersistenceLayer.Repositories.Entities;
using MetroMiles.DomainLayer.Entities.Enums;

namespace MetroMiles.DomainLayer.Entities;

public class Car : Entity<Guid>
{
    public Guid ModelId { get; set; }
    public int Kilometer { get; set; }
    public int Mile { get; set; }
    public short ModelYear { get; set; }
    public string Plate { get; set; }
    public short MinFindexScore { get; set; }
    public  CarStatus Status { get;set; }
    public virtual ICollection<Model> Models { get; set; }

    public Car()
    {
        Models = new HashSet<Model>();
    }

    public Car( Guid id,Guid modelId, int kilometer, int mile, short modelYear, string plate, short minFindexScore, CarStatus status) : this()
    {
        Id = id;
        ModelId = modelId;
        Kilometer = kilometer;
        Mile = mile;
        ModelYear = modelYear;
        Plate = plate;
        MinFindexScore = minFindexScore;
        Status = status;
    }
}