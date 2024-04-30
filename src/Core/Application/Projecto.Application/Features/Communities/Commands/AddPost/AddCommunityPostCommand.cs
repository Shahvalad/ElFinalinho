using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Communities.Commands.AddPost
{
    public record AddCommunityPostCommand(CreateCommunityPostDto CreateCommunityPostDto, string UserId, int CommunityId) : IRequest<int>;
    public class AddCommunityPostCommandHandler : IRequestHandler<AddCommunityPostCommand, int>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public AddCommunityPostCommandHandler(IDataContext context, IMapper mapper, IImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<int> Handle(AddCommunityPostCommand request, CancellationToken cancellationToken)
        {
            var post = new CommunityPost
            {
                Title = request.CreateCommunityPostDto.Title,
                isSpoiler = request.CreateCommunityPostDto.isSpoiler,
                UserId = request.UserId,
                CommunityId = request.CommunityId,
            };
            var imageName = await _imageService.CreateImageAsync("CommunityPosts", request.CreateCommunityPostDto.Image);
            post.CommunityPostImage = new CommunityPostImage { FileName = imageName };
            post.UserId = request.UserId;
            post.CommunityId = request.CommunityId;
            await _context.CommunityPosts.AddAsync(post);
            await _context.SaveChangesAsync(cancellationToken);
            return post.Id;
        }
    }
}
