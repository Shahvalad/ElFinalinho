using Microsoft.AspNetCore.Identity;
using Projecto.Application.Dtos.ProfileDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Profile.Commands.Edit
{
    public record EditProfileCommand(ClaimsPrincipal User, EditProfileDto EditProfileDto) : IRequest;
    public class EditProfileCommandHandler : IRequestHandler<EditProfileCommand>
    {
        private readonly UserManager<AppUser> _userManager;

        public EditProfileCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(EditProfileCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.GetUserAsync(request.User)??throw new Exception("user not found!");
            existingUser.FirstName = request.EditProfileDto.FirstName;
            existingUser.LastName = request.EditProfileDto.LastName;
            existingUser.Bio = request.EditProfileDto.Bio;
            existingUser.Email = request.EditProfileDto.Email;
            await _userManager.UpdateAsync(existingUser);
        }
    }
}
