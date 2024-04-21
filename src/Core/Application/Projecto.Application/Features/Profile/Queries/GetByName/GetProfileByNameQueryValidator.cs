namespace Projecto.Application.Features.Profile.Queries.GetByName
{
    public class GetProfileByNameQueryValidator : AbstractValidator<GetProfileByNameQuery>
    {
        public GetProfileByNameQueryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters");
        }
    }
}
