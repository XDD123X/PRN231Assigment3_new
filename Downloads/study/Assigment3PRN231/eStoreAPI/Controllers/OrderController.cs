using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject;
using BusinessObject.DTO;

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // GET: api/order/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return order;
        }

        // GET: api/order/member/{memberId}
        [HttpGet("member/{memberId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrderByMemberId(string memberId)
        {
            var orders = await _context.Orders.Where(o => o.MemberId.Equals( memberId)).ToListAsync();
            if (orders == null || !orders.Any())
            {
                return NotFound("No orders found for this member.");
            }
            return orders;
        }

        // POST: api/order
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDTO orderDTO)
        {
            var order = new Order
            {
                OrderDate = orderDTO.OrderDate,
                RequiredDate = orderDTO.RequiredDate,
                ShippedDate = orderDTO.ShippedDate,
                Freight = orderDTO.Freight
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderDTO orderDTO)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            order.OrderDate = orderDTO.OrderDate;
            order.RequiredDate = orderDTO.RequiredDate;
            order.ShippedDate = orderDTO.ShippedDate;
            order.Freight = orderDTO.Freight;

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }


        // DELETE: api/order/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            _context.OrderDetails.RemoveRange(order.OrderDetails);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
