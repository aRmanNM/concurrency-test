using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Services;

namespace webapi.Controllers
{
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ProductManagement _productManagement;
        private readonly AppDbContext _context;

        public TestController(ProductManagement productManagement, AppDbContext context)
        {
            _context = context;
            _productManagement = productManagement;
        }

        [HttpGet("buy/{productId}")]
        public async Task<IActionResult> BuyProduct(int productId)
        {

            var stat = await _productManagement.ProcessSale(productId);

            if (stat)
            {
                Console.WriteLine("bought");
            }
            else
            {
                Console.WriteLine("Could not buy");
            }


            return Ok();
        }

        [HttpGet("getQuantity/{productId}")]
        public async Task<IActionResult> GetQuantity(int productId)
        {
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == productId);

            Console.WriteLine(product.Quantity);

            return Ok();
        }

    }
}