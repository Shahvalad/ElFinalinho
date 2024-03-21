using Microsoft.AspNetCore.Http;

namespace Projecto.Application.Services.ImageService
{
    public interface IImageService
    {
        public Task<string> CreateImageAsync(string folder, IFormFile file);
        public Task<bool> DeleteImage(string folder, string fileName);
    }
}
