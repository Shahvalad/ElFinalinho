using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Profile.Commands.Edit
{
    public class EditProfileCommandValidator : AbstractValidator<EditProfileCommand>
    {
        public EditProfileCommandValidator()
        {
            RuleFor(v => v.User)
                .NotEmpty().WithMessage("Id is required.")
                .NotNull().WithMessage("Id is required.");

            RuleFor(v => v.EditProfileDto)
                .NotEmpty().WithMessage("EditProfileDto is required.")
                .NotNull().WithMessage("EditProfileDto is required.");

            RuleFor(v=>v.EditProfileDto!.FirstName)
                .NotEmpty().WithMessage("FirstName is required.")
                .NotNull().WithMessage("FirstName is required.")
                .MinimumLength(2).WithMessage("FirstName must be at least 2 characters")
                .MaximumLength(20).WithMessage("FirstName must be less than 20 characters"); ;

            RuleFor(v=>v.EditProfileDto!.LastName)
                .NotEmpty().WithMessage("LastName is required.")
                .NotNull().WithMessage("LastName is required.")
                .MinimumLength(2).WithMessage("Lastname must be at least 2 characters")
                .MaximumLength(20).WithMessage("Lastname must be less than 20 characters");


            RuleFor(v=>v.EditProfileDto!.Bio)
                .MaximumLength(50).WithMessage("Bio must not exceed 50 characters.");

        }
    }
}
