using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Application.Operations.Adverts.Commands.UpdateAdvert;

public class UpdateAdvertCommandHandler : IRequestHandler<UpdateAdvertCommand, AdvertsResponse>
{
    private readonly IMapper _mapper;
    private readonly IAdvertRepository _advertRepository;

    public UpdateAdvertCommandHandler(IMapper mapper, IAdvertRepository advertRepository)
    {
        _mapper = mapper;
        _advertRepository = advertRepository;
    }

    public async Task<AdvertsResponse> Handle(UpdateAdvertCommand request, CancellationToken cancellationToken)
    {
        var advertisement =
            await _advertRepository.FindAdvertByIdAsync(request.AdvertId, cancellationToken)
            ?? throw new NotFoundException(nameof(Advert), request.AdvertId);

        if (advertisement.UserId != request.CurrentUserId)
            throw new AccessDeniedException(nameof(Advert), request.AdvertId);

        advertisement.Description = request.Description ?? advertisement.Description;
        advertisement.Price = request.Price ?? advertisement.Price;

        await _advertRepository.UpdateAdvertAsync(advertisement, cancellationToken);
        return _mapper.Map<AdvertsResponse>(advertisement);
    }
}