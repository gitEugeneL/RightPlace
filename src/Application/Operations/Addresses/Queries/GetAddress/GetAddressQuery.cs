using MediatR;

namespace Application.Operations.Addresses.Queries.GetAddress;

public record GetAddressQuery(Guid Id) : IRequest<AddressResponse>;
