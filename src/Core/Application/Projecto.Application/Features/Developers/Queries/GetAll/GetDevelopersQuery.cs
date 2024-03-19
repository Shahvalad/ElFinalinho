using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Developers.Queries.GetAll
{
    public record GetDevelopersQuery : IRequest<IEnumerable<GetDeveloperDto>>;
    public class GetDevelopersQueryHandler : IRequestHandler<GetDevelopersQuery, IEnumerable<GetDeveloperDto>>
    {
        private readonly IMapper _mapper;
        private readonly IDataContext _context;

        public GetDevelopersQueryHandler(IMapper mapper, IDataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<GetDeveloperDto>> Handle(GetDevelopersQuery request, CancellationToken cancellationToken)
        {
            var developers = await _context.Developer.AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<GetDeveloperDto>>(developers);
        }
    }
}
