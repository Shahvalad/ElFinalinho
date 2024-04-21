using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Dtos.ReviewDtos
{
    public class PaginatedReviewsDto
    {
        public List<Review> Reviews { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
