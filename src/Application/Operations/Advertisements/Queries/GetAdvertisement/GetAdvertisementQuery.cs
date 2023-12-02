using MediatR;

namespace Application.Operations.Advertisements.Queries.GetAdvertisement;

public record GetAdvertisementQuery(Guid Id) : IRequest<AdvertisementResponse>;