using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Projecto.Application.Features.Profile.Commands.ChangeProfilePicture
{
    public record ChangeProfilePictureCommand(string UserId, IFormFile file) : IRequest<Result<GetProfileDto>>;

    public class ChangeProfilePictureCommandHandler : IRequestHandler<ChangeProfilePictureCommand, Result<GetProfileDto>>
    {
        private readonly IImageService _imageService;
        private readonly IDataContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ChangeProfilePictureCommandHandler(UserManager<AppUser> userManager, IDataContext context, IImageService imageService)
        {
            _userManager = userManager;
            _context = context;
            _imageService = imageService;
        }

        public async Task<Result<GetProfileDto>> Handle(ChangeProfilePictureCommand request, CancellationToken cancellationToken)
        {
            var user = await GetUser(request.UserId, cancellationToken);
            if (user == null)
            {
                return Result<GetProfileDto>.Failure(new[] { "User not found" });
            }

            await DeleteExistingProfilePicture(user);

            var newProfilePictureName = await CreateNewProfilePicture(request.file);
            UpdateUserProfilePicture(user, newProfilePictureName, request.UserId);

            await _context.SaveChangesAsync(cancellationToken);

            var getProfileDto = CreateProfileDto(user, newProfilePictureName);
            return Result<GetProfileDto>.Success(getProfileDto);
        }

        private async Task<AppUser> GetUser(string userId, CancellationToken cancellationToken)
        {
            return await _userManager.Users
                .Include(u => u.ProfilePicture)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        private async Task DeleteExistingProfilePicture(AppUser user)
        {
            if (user.ProfilePicture != null)
            {
                await _imageService.DeleteImage("ProfilePictures", user.ProfilePicture.FileName);
            }
        }

        private async Task<string> CreateNewProfilePicture(IFormFile file)
        {
            return await _imageService.CreateImageAsync("ProfilePictures", file);
        }

        private void UpdateUserProfilePicture(AppUser user, string newProfilePictureName, string userId)
        {
            user.ProfilePicture = new AppUserProfilePicture()
            {
                FileName = newProfilePictureName,
                UserId = userId
            };
            var existingProfilePicture = _context.AppUserProfilePictures.FirstOrDefault(p => p.UserId == userId);
            if (existingProfilePicture!=null)
            {
                _context.AppUserProfilePictures.Remove(existingProfilePicture);
            }
            _context.AppUserProfilePictures.AddAsync(user.ProfilePicture);
        }

        private GetProfileDto CreateProfileDto(AppUser user, string newProfilePictureName)
        {
            return new GetProfileDto
            (
                user.UserName,
                user.FirstName,
                user.LastName,
                user.Bio,
                user.Email,
                newProfilePictureName,
                user.MemberSince,
                new List<GetGameDto>()
            );
        }

    }

}
