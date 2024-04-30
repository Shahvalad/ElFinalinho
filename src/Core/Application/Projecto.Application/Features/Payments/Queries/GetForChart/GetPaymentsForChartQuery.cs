using AngleSharp.Dom;
using Projecto.Application.Dtos.PaymentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Payments.Queries.GetForChart
{
    public record GetPaymentsForChartQuery() : IRequest<PaymentsForChartDto>;
    public class GetPaymentsForChartQueryHandler : IRequestHandler<GetPaymentsForChartQuery, PaymentsForChartDto>
    {
        private readonly IDataContext _context;
        private readonly UserManager<AppUser> _userManager;
        public GetPaymentsForChartQueryHandler(IDataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<PaymentsForChartDto> Handle(GetPaymentsForChartQuery request, CancellationToken cancellationToken)
        {
            var payments = await _context.Payments
                .Select(p => new { p.PaymentDate, p.Amount })
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var groupedPayments = payments
                .GroupBy(p => new { Year = p.PaymentDate.Year, Month = p.PaymentDate.Month })
                .Select(g => new PaymentForChartDto (new DateTime(g.Key.Year, g.Key.Month, 1), g.Count() ))
                .OrderBy(x => x.Date)
                .ToList();

            long totalAmount = payments.Sum(p => p.Amount).GetValueOrDefault();

            var totalUsers = _userManager.Users.Count();

            return new PaymentsForChartDto { Payments = groupedPayments, TotalAmount = totalAmount, TotalUsers = totalUsers };

        }


    }
}
