using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Projecto.Domain.Models;

namespace Projecto.Application.Profiles
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<Genre, GetGenreDto>();
            CreateMap<Genre, UpdateGenreDto>().ReverseMap();
            CreateMap<GetGenreDto, UpdateGenreDto>();
        }
    }
}
