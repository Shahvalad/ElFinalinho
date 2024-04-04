namespace Projecto.Application.Features.Games.Commands.AddKeys
{
    public class AddKeysCommandValidator : AbstractValidator<AddKeysCommand>
    {
        public AddKeysCommandValidator()
        {
            RuleFor(p => p.id)
                .NotNull()
                .WithMessage("Id is required.")
                .GreaterThan(0)
                .WithMessage("Id must be positivie number");

            RuleFor(p => p.Keys)
            .NotNull()
            .WithMessage("Keys is required.")
            .Must(keys => keys.Count > 0)
            .WithMessage("Keys must not be empty.")
            .Must(keys => keys.All(key => key != null))
            .WithMessage("Keys must not contain null elements.")
            .Must(keys => keys.Distinct().Count() == keys.Count)
            .WithMessage("Keys must be unique.")
            .Must(keys => keys.All(key => key.Length >= 5))
            .WithMessage("All keys must be at lease 5 characters long.");
        }
    }
}
