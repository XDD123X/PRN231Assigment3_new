using Microsoft.AspNetCore.Mvc;

namespace eStore.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
