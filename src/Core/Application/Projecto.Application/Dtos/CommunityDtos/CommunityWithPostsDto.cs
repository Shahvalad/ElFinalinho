using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Dtos.CommunityDtos
{
    public record CommunityWithPostsDto(
        int Id, 
        string Name, 
        int ThreadsCount, 
        string ImageName,
        List<CommunityPost> Posts);
}
