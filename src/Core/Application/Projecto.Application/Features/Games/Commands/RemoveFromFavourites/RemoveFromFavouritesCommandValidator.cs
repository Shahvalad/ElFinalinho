using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Games.Commands.RemoveFromFavourites
{
    public class RemoveFromFavouritesCommandValidator : AbstractValidator<RemoveFromFavouritesCommand>
    {
        public RemoveFromFavouritesCommandValidator()
        {
            RuleFor(v => v.GameId)
                .NotEmpty().WithMessage("GameId is required.")
                .GreaterThan(0).WithMessage("GameId must be greater than 0.");

            RuleFor(v => v.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .NotNull().WithMessage("UserId is required.");
        }
    }
}
