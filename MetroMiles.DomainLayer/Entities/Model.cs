﻿using Core.PersistenceLayer.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroMiles.DomainLayer.Entities;

public class Model :  Entity<Guid>
{
    public Guid BrandId { get; set; }
    public string Name { get; set; }
    public decimal DailyPrice { get; set; }
    public string ImageUrl   { get; set; }
}

public class Transmission : Entity<Guid>
{
    public string Name { get; set; }
}

public class Fuel : Entity<Guid>
{
    public string Name { get; set; }
}

public class Car : Entity<Guid>
{
    public Guid ModelId { get; set; }
    public int Kilometer { get; set; }
    public int Mile { get; set; }
    public short ModelYear { get; set; }
    public string Plate { get; set; }
    public short MinFindexScore { get; set; }
    public  CarStatus Status { get;set; }
}