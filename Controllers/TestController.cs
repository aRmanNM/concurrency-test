using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using webapi.Data;
using webapi.Models;

namespace webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly ILogger logger;
        private readonly IDistributedLockFactory factory;
        public TestController(AppDbContext context, ILogger<Product> logger, IDistributedLockFactory factory)
        {
            this.factory = factory;
            this.logger = logger;
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int id)
        {

            var product = await context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();

            var b = new Buyer()
            {
                ProductId = id,
                IsPaid = false
            };

            await context.Buyers.AddAsync(b);

            var resource = product.Id;
            var expiry = TimeSpan.FromSeconds(30);

            using (var redLock = await factory.CreateLockAsync(resource.ToString(), expiry)) // there are also non async Create() methods
            {
                // make sure we got the lock
                if (redLock.IsAcquired)
                {
                    if (product.Count > 0)
                    {
                        product.Count--;
                        logger.LogInformation($"bought one {product.Name} now we have {product.Count}");
                        b.IsPaid = true;
                        await context.SaveChangesAsync();
                        return Ok();
                    }
                }
            }

            await context.SaveChangesAsync();
            logger.LogInformation($"bad request - {product.Count}");

            return BadRequest();
        }

        [HttpGet("getbuyers")]
        public async Task<IActionResult> GetBuyer([FromQuery] int id)
        {
            var buyers = await context.Buyers.Where(p => p.ProductId == id && p.IsPaid).ToListAsync();


            return Ok(buyers.Count());
        }
    }
}