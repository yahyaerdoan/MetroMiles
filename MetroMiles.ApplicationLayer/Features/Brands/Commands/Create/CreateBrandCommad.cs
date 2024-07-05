using AutoMapper;
using MediatR;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroMiles.ApplicationLayer.Features.Brands.Commands.Create;

public class CreateBrandCommand : IRequest<CreatedBrandResponse>
{
    public string Name { get; set; }
    public string Description { get; set; }

    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, CreatedBrandResponse>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public CreateBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<CreatedBrandResponse> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            Brand brand = new();
            brand.Id = new Guid(Guid.NewGuid().ToString());
            brand.Name = request.Name;
            brand.Description = request.Description;
            var result = await _brandRepository.AddAsync(brand);

            CreatedBrandResponse createdBrandResponse = new(); 
            createdBrandResponse.Id = result.Id; 
            createdBrandResponse.Name = request.Name; 
            createdBrandResponse.Description = request.Description;
       
            return createdBrandResponse;
        }
    }
}
