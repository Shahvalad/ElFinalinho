using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Profile.Queries.Get
{
    public class GetProfileQueryValidator : AbstractValidator<GetProfileQuery>
    {
        public GetProfileQueryValidator()
        {
            RuleFor(v => v.User)
                .NotEmpty().WithMessage("User is required.")
                .NotNull().WithMessage("User is required.");
        }
    }
}
