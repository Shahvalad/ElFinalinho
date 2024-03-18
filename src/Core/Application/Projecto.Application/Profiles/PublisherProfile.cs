using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projecto.Application.Dtos.Publisher;

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
                .ForMember(dest => dest.Logo, opt => opt.Ignore()); // Ignore the Logo property
        }
    }
}
