using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Services.ImageService
{
    public interface IImageService
    {
        public Task<string> CreateImageAsync(string folder, IFormFile file);
        public Task DeleteImage(string folder, string fileName);
    }
}
