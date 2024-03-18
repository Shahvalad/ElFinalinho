using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Genres.Queries.GetGenre
{
    public class GetGenreQueryValidator : AbstractValidator<GetGenreQuery>
    {
        public GetGenreQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .WithMessage("Id is required.")
                .GreaterThan(0)
                .WithMessage("Id must be a positive integer.");
        }
    }
}
