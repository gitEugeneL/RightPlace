using MediatR;

namespace Application.Operations.Adverts.Queries.GetAdvert;

public record GetAdvertQuery(Guid Id) : IRequest<AdvertsResponse>;