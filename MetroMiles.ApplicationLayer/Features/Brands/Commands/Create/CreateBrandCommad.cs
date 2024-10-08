﻿using AutoMapper;
using Core.ApplicationLayer.Pipelines.Cachings.Abstractions;
using Core.ApplicationLayer.Pipelines.Loggings.Abstractions;
using Core.ApplicationLayer.Pipelines.Transactions.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Brands.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroMiles.ApplicationLayer.Features.Brands.Commands.Create;

public class CreateBrandCommand : IRequest<CreatedBrandResponse>, ITransactionAddRequest, ICacheRemoveRequest, ILogAddRequest
{
    #region CreateBrandCommand & ICacheRemoveRequest Properties  
    public string Name { get; set; }
    public string Description { get; set; }
    public string CacheKey => "";
    public bool ByPassCache => false;
    public string? CacheGroupKey => "GetBrands";
    #endregion

    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, CreatedBrandResponse>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        private readonly BrandBusinessRules _brandBusinessRules;

        public CreateBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper, BrandBusinessRules brandBusinessRules)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _brandBusinessRules = brandBusinessRules;
        }

        public async Task<CreatedBrandResponse> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            await _brandBusinessRules.BrandNameCannotBeDuplicatedWhenInserted(request.Name);
            var brand = _mapper.Map<Brand>(request);           
            await _brandRepository.AddAsync(brand);            
            CreatedBrandResponse createdBrandResponse = _mapper.Map<CreatedBrandResponse>(brand);
            return createdBrandResponse;
        }
    }
}
