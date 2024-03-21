namespace Projecto.Application.Profiles
{
    class DeveloperProfile : Profile
    {
        public DeveloperProfile()
        {
            CreateMap<Developer, GetDeveloperDto>();
            CreateMap<Developer, GetDeveloperDto>()
                .ForMember(dest => dest.Logo, opt => opt.Ignore());
            CreateMap<CreateDeveloperDto, Developer>()
                .ForMember(dest => dest.Logo, opt => opt.Ignore());
            CreateMap<GetDeveloperDto, UpdateDeveloperDto>().ReverseMap();
            CreateMap<UpdateDeveloperDto, Developer>()
                .ForMember(dest => dest.Logo, opt => opt.Ignore());
        }
    }
}
