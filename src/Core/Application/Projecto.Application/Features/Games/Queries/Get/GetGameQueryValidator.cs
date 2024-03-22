namespace Projecto.Application.Features.Games.Queries.Get
{
    public class GetGameQueryValidator : AbstractValidator<GetGameQuery>
    {
        public GetGameQueryValidator()
        {
            RuleFor(x=>x.Id)
                .NotNull().WithMessage("Id is required")
                .NotEmpty().WithMessage("Id is required")
                .GreaterThan(0).WithMessage("Id must be positive number");  
        }
    }
}
