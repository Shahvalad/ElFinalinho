using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Carts.Commands.AddToCart
{
    public class AddToCartCommandValidator : AbstractValidator<AddToCartCommand>
    {
        public AddToCartCommandValidator()
        {
            RuleFor(v => v.GameId)
                .NotEmpty().WithMessage("GameId is required.")
                .GreaterThan(0).WithMessage("GameId must be greater than 0.");

            RuleFor(v => v.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            RuleFor(v => v.CurrentCart)
                .NotEmpty().WithMessage("CartId is required.")
                .NotNull().WithMessage("CartId is required.");
        }
    }
}
