using Ganss.Xss;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Reviews.Commands.Create
{
    public record CreateReviewCommand(bool IsLiked, string Comment, string UserId, int GameId) : IRequest;
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand>
    {
        private readonly IDataContext _context;
        public CreateReviewCommandHandler(IDataContext context)
        {
            _context = context;
        }
        public async Task Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var sanitizer = new HtmlSanitizer();
            request = request with { Comment = sanitizer.Sanitize(request.Comment) };
            await _context.Reviews.AddAsync(new Review()
            {
                IsLiked = request.IsLiked,
                Comment = request.Comment,
                UserId = request.UserId,
                GameId = request.GameId
            }, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
