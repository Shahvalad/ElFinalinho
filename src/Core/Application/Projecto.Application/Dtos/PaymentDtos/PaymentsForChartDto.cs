using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Dtos.PaymentDtos
{
    public class PaymentsForChartDto
    {
        public List<PaymentForChartDto> Payments { get; set; }
        public long TotalAmount { get; set; }
        public int TotalUsers { get; set; }
        public int UsersRegisteredThisYear { get; set; }
        public List<GetGameDto> PopularGames { get; set; }

    }
}
