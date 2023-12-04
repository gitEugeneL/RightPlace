using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IInformationRepository
{
    Task UpdateInformationAsync(Information information, CancellationToken cancellationToken);

    Task<Information> CreateInformationAsync(Information information, CancellationToken cancellationToken);

    Task<Information?> FindInformationByIdAsync(Guid id, CancellationToken cancellationToken);
}
