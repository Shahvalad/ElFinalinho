namespace Projecto.Application.Profiles
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<Genre, GetGenreDto>();
            CreateMap<Genre, UpdateGenreDto>().ReverseMap();
            CreateMap<GetGenreDto, UpdateGenreDto>();
            CreateMap<CreateGenreDto, Genre>();
        }
    }
}
