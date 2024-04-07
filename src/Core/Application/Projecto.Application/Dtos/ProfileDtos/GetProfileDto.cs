namespace Projecto.Application.Dtos.ProfileDtos
{
    public record GetProfileDto(string FirstName,
                                string LastName,
                                string Bio,
                                string Email,
                                DateTime RegistrationDate,
                                List<GetGameDto> Games);
}
