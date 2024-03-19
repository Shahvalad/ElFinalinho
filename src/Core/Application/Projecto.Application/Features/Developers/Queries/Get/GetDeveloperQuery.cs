using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Developers.Queries.Get
{
    public record GetDeveloperQuery(int? Id) : IRequest<GetDeveloperDto>;
    public class GetDeveloperQueryHandler : IRequestHandler<GetDeveloperQuery, GetDeveloperDto>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;

        public GetDeveloperQueryHandler(IMapper mapper, IDataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<GetDeveloperDto> Handle(GetDeveloperQuery request, CancellationToken cancellationToken)
        {
            var developer = await _context.Developer.Include(d => d.Logo).FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
                ?? throw new DeveloperNotFoundException("There is no developer with such id!");
            return _mapper.Map<GetDeveloperDto>(developer);
        }
    }
}
