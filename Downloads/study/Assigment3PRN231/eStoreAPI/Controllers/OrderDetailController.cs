using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult> getReport(DateTime sd,DateTime ed)
        {
            List<ReportDTO> reportDTOs = new List<ReportDTO>();
            try
            {
                reportDTOs = await _context.OrderDetails
                    .Include(x => x.Order).Where(x=>x.Order.OrderDate>=sd && x.Order.OrderDate<=ed)
                    .GroupBy(x => x.Order.MemberId)
                    .Select(g => new ReportDTO
                    {
                        afterSale = g.Sum(x => x.UnitPrice * x.Quantity * (1 - x.Discount)),
                        beforeSale = g.Sum(x => x.UnitPrice * x.Quantity)
                    })
                    .OrderBy(r => r.beforeSale) 
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Ok(reportDTOs);
        }

    }
}
