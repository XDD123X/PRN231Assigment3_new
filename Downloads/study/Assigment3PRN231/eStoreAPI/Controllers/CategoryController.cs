using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult> getAllCategory()
        {
            try
            {
                List<CategoryDTO> categories = _context.Categories.Select(x => new CategoryDTO
                {
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName
                }).ToList();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex);
            }
        }
    }
}
