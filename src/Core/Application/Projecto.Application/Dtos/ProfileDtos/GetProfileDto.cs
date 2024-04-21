namespace Projecto.Application.Dtos.ProfileDtos
{
    public record GetProfileDto(string Username,
                                string? FirstName,
                                string? LastName,
                                string? Bio,
                                string Email,
                                string? ProfilePicture,
                                DateTime RegistrationDate,
                                List<GetGameDto> Games);
}
