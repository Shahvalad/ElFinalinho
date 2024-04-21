namespace Projecto.Application.Features.Profile.Queries.GetByName
{
    //TODO : ADD VALIDATION
    public record GetProfileByNameQuery(string Name) : IRequest<GetProfileDto>;
    public class GetProfileByNameQueryHandler : IRequestHandler<GetProfileByNameQuery, GetProfileDto>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public GetProfileByNameQueryHandler(IMapper mapper, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<GetProfileDto> Handle(GetProfileByNameQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.Name, cancellationToken) 
                       ?? throw new UserNotFoundException("User with such name not found!");
            var profileDto = _mapper.Map<GetProfileDto>(user);
            return profileDto;
        }
    }
}
