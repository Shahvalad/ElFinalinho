using static Projecto.Application.Common.Helpers.Helper;
namespace Projecto.MVC.Areas.Admin.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> CreateImageAsync(string folder, IFormFile file)
        {
            const int maxFileSize = 2 * 1024 * 1024; // 2MB in bytes

            if (file.Length > maxFileSize)
            {
                throw new Exception("File size exceeded 2MB.");
            }

            var uniqueImageName = GetUniqueFileName(file.FileName);
            string path = Path.Combine(_environment.WebRootPath, $"Images/{folder}", uniqueImageName);

            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return uniqueImageName;
        }

        public async Task<bool> DeleteImage(string folder, string fileName)
        {
            string path = Path.Combine(_environment.WebRootPath, "Images", $"{folder}", fileName);
            if (!File.Exists(path)) return false;
            File.Delete(path);
            return true;
        }
    }
}
