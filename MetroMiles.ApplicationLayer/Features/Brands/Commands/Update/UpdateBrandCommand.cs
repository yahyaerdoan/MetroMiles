﻿using AutoMapper;
using Core.ApplicationLayer.Pipelines.Cachings.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroMiles.ApplicationLayer.Features.Brands.Commands.Update;

public class UpdateBrandCommand : IRequest<UpdatedBrandResponse>, ICacheRemoveRequest
{
    #region UpdateBrandCommand & ICacheRemoveRequest Properties
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CacheKey => "";
    public bool ByPassCache => false;
    public string? CacheGroupKey => "GetBrands";
    #endregion

    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, UpdatedBrandResponse>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public UpdateBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<UpdatedBrandResponse> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            Brand? brand = await _brandRepository.GetAsync(predicate: b => b.Id == request.Id, cancellationToken: cancellationToken);
            brand = _mapper.Map(request, brand);
            await _brandRepository.UpdateAsync(brand);
            UpdatedBrandResponse response = _mapper.Map<UpdatedBrandResponse>(brand);
            return response;
        }
    }
}
