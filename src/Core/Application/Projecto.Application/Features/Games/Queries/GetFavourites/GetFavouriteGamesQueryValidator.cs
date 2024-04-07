using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Games.Queries.GetFavourites
{
    public class GetFavouriteGamesQueryValidator : AbstractValidator<GetFavouriteGamesQuery>
    {
        public GetFavouriteGamesQueryValidator()
        {
            RuleFor(v => v.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .NotNull().WithMessage("UserId is required.");
        }
    }
}
