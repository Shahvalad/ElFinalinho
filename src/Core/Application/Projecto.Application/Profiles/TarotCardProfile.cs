using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using Projecto.Application.Dtos.TarotCardDtos;
using Projecto.Application.Features.TarotCards.Commands.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Profiles
{
    public class TarotCardProfile : Profile
    {
        public TarotCardProfile()
        {
            CreateMap<TarotCard, GetTarotCardDto>().ReverseMap();
        }
    }
}
