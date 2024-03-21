namespace Projecto.Application.Profiles
{
    public class PublisherProfile : Profile
    {
        public PublisherProfile()
        {
            CreateMap<Publisher, GetPublisherDto>();
            CreateMap<Publisher, GetPublisherDto>()
                .ForMember(dest => dest.Logo, opt => opt.Ignore());
            CreateMap<CreatePublisherDto, Publisher>()
                .ForMember(dest => dest.Logo, opt => opt.Ignore());
            CreateMap<GetPublisherDto, UpdatePublisherDto>().ReverseMap();
            CreateMap<UpdatePublisherDto, Publisher>()
                .ForMember(dest => dest.Logo, opt => opt.Ignore());
        }
    }
}
