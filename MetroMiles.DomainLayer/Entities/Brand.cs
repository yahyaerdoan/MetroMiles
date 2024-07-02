using Core.PersistenceLayer.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroMiles.DomainLayer.Entities;

public class Brand : Entity<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Brand()
    {

    }
    public Brand(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }   
}
