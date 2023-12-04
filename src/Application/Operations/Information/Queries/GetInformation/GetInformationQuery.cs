using MediatR;

namespace Application.Operations.Information.Queries.GetInformation;

public record GetInformationQuery(Guid Id) : IRequest<InformationResponse>;
