﻿using Core.PersistenceLayer.Repositories.IRepositories;
using Core.SecurityLayer.Entities;

namespace MetroMiles.ApplicationLayer.Services.Repositories;

public interface IOperationClaimRepository : IAsyncRepository<OperationClaim, int>//, IRepository<OperationClaim, int>
{
}
