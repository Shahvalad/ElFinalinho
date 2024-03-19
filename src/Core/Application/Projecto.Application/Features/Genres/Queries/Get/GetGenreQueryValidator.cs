

namespace Projecto.Application.Features.Genres.Queries.Get
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
