using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Developers.Queries.Get
{
    public class GetDeveloperQueryValidator : AbstractValidator<GetDeveloperQuery>
    {
        public GetDeveloperQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .WithMessage("Id is required.")
                .GreaterThan(0)
                .WithMessage("Id must be a positive integer.");
        }
    }
}
