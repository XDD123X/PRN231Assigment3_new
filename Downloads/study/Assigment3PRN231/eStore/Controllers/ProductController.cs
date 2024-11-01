using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using BusinessObject.DTO;

namespace eStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _client;
        private readonly string ProductApiUrl = "https://localhost:7064/api/Product";

        public ProductController()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _client.GetAsync(ProductApiUrl);

            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                List<ProductDTO> listProduct = JsonSerializer.Deserialize<List<ProductDTO>>(strData, options);
                return View(listProduct);
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error fetching products: {response.StatusCode} - {errorContent}");
            return View("Error");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"{ProductApiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                ProductDTO product = JsonSerializer.Deserialize<ProductDTO>(strData, options);
                return View(product);
            }
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error fetching product for edit: {response.StatusCode} - {errorContent}");
            return NotFound();
        }
    }
}
