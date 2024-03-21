namespace Projecto.Application.Features.Developers.Commands.Edit
{
    public class EditDeveloperCommandValidator : AbstractValidator<EditDeveloperCommand>
    {
        public EditDeveloperCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty()
                .WithMessage("Id is required.")
                .NotNull()
                .WithMessage("Id is required");

            RuleFor(x => x.DeveloperDto.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .NotNull()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.DeveloperDto.Description)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.DeveloperDto.Logo)
                .Must(x => x?.Length < Math.Pow(2, 20) * 2 || x is null)
                .WithMessage("Logo must not exceed 2MB.");

        }
    }
}
