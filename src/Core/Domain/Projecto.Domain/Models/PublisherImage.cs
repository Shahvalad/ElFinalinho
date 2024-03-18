

using Projecto.Domain.Models.Common;

namespace Projecto.Domain.Models
{
    public class PublisherImage : BaseImage
    {
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; } = null!;
    }
}
