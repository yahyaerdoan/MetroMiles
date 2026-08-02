using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Cars.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;
using MetroMiles.DomainLayer.Entities.Enums;

using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

using static MetroMiles.ApplicationLayer.Features.Cars.Constants.CarsOperationClaims;

namespace MetroMiles.ApplicationLayer.Features.Cars.Commands.Create;

public class CreateCarCommand : IRequest<OperationDataResult<CreatedCarResponse>>, ISecureAddRequest
{
    public Guid ModelId { get; set; }
    public int Kilometer { get; set; }
    public int Mile { get; set; }
    public short ModelYear { get; set; }
    public required string Plate { get; set; }
    public short MinFindexScore { get; set; }
    public CarStatus Status { get; set; }
    public string[] Roles => [Admin, Write, Add];

    public class CreateCarCommandHandler(ICarRepository carRepository, IMapper mapper, CarBusinessRules carBusinessRules) : IRequestHandler<CreateCarCommand, OperationDataResult<CreatedCarResponse>>
    {
        private readonly ICarRepository _carRepository = carRepository;
        private readonly IMapper _mapper = mapper;
        private readonly CarBusinessRules _carBusinessRules = carBusinessRules;

        public async Task<OperationDataResult<CreatedCarResponse>> Handle(CreateCarCommand request, CancellationToken cancellationToken)
        {
            var modelCheck = await _carBusinessRules.ModelIdShouldExistWhenSelected(request.ModelId);
            if (!modelCheck.IsSuccessful)
            {
                return modelCheck.ToErrorDataResult<CreatedCarResponse>();
            }

            var car = _mapper.Map<Car>(request);
            await _carRepository.AddAsync(car);
            return Result.Success(_mapper.Map<CreatedCarResponse>(car));
        }
    }
}
