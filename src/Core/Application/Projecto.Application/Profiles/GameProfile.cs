using Projecto.Application.Dtos.GameDtos;

namespace Projecto.Application.Profiles
{
    public class GameProfile : Profile
    {
        public GameProfile()
        {
            CreateMap<CreateGameDto, Game>()
               .ForMember(dest => dest.Images, opt => opt.Ignore())
               .ForMember(dest=> dest.Developer, opt => opt.Ignore())
               .ForMember(dest=>dest.GameGenres, opt => opt.Ignore());
            CreateMap<Game, GetGameDto>()
                .ForMember(dest => dest.Images, GetGameDto => GetGameDto.MapFrom(src => src.Images.Select(x => x.ImageFile)));
            CreateMap<GetGameDto, UpdateGameDto>()
                .ForMember(dest=>dest.Images, opt=>opt.Ignore());
        }
    }
}
