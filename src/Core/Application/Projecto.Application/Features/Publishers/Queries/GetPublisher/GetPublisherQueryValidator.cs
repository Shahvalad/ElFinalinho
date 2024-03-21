namespace Projecto.Application.Features.Publishers.Queries.GetPublisher
{
    public class GetPublisherQueryValidator : AbstractValidator<GetPublisherQuery>
    {
        public GetPublisherQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .WithMessage("Id is required.")
                .GreaterThan(0)
                .WithMessage("Id must be a positive integer.");
        }
    }
}
