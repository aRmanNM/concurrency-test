using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using webapi.Data;
using webapi.Models;
using webapi.Services;

namespace webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        public TestController(ProductManagement productManagement, AppDbContext context, ILogger<Product> logger)
        {
            ProductManagement = productManagement;
            Context = context;
            Logger = logger;
        }

        public ProductManagement ProductManagement { get; }
        public AppDbContext Context { get; }
        public ILogger Logger { get; }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int id)
        {

            var res = await ProductManagement.Process(id);

            if (res)
            {
                Logger.LogInformation("succeeded");
            }
            else
            {
                Logger.LogInformation("failed");
            }

            return Ok();
        }

        [HttpGet("getbuyers")]
        public async Task<IActionResult> GetBuyer([FromQuery] int id)
        {
            var buyers = await Context.Buyers.Where(p => p.ProductId == id).ToListAsync();

            return Ok(buyers);
        }
    }
}