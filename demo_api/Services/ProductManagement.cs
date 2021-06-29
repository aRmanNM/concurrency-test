using System.Linq;
using System.Threading.Tasks;
using Medallion.Threading.Redis;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;

namespace webapi.Services
{
    public class ProductManagement
    {
        private readonly AppDbContext _context;
        private readonly RedisDistributedSynchronizationProvider _redisDistributedSynchronizationProvider;

        public ProductManagement(AppDbContext context, RedisDistributedSynchronizationProvider redisDistributedSynchronizationProvider)
        {
            _context = context;
            _redisDistributedSynchronizationProvider = redisDistributedSynchronizationProvider;
        }

        public async Task<bool> ProcessSale(int productId)
        {
            var productSale = new ProductSale();

            var redislock = _redisDistributedSynchronizationProvider.CreateLock($"PRODUCT_{productId}");
            using (await redislock.AcquireAsync())
            {
                var product = await _context.Products.Where(p => p.Id == productId).FirstOrDefaultAsync();

                productSale.ProductId = productId;
                productSale.Status = false;

                await _context.ProductSales.AddAsync(productSale);

                if (product.Quantity > 0)
                {
                    product.Quantity--;
                    productSale.Status = true;
                }
            }

            await _context.SaveChangesAsync();

            if (!productSale.Status)
            {
                return false;
            }

            return true;
        }
    }
}