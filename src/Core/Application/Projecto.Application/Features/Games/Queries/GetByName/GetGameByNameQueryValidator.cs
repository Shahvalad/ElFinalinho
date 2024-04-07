using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Games.Queries.GetByName
{
    public class GetGameByNameQueryValidator : AbstractValidator<GetGameByNameQuery>
    {
        public GetGameByNameQueryValidator()
        {
            RuleFor(v => v.searchTerm)
                .NotEmpty().WithMessage("Name is required.")
                .NotNull().WithMessage("Name is required.");
        }
    }
}
