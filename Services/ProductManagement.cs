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
        public ProductManagement(AppDbContext context, RedisDistributedSynchronizationProvider redisDistributedSynchronizationProvider)
        {
            Context = context;
            RedisDistributedSynchronizationProvider = redisDistributedSynchronizationProvider;
        }

        public AppDbContext Context { get; }
        public RedisDistributedSynchronizationProvider RedisDistributedSynchronizationProvider { get; }

        public async Task<bool> Process(int id)
        {
            var b = new Buyer();

            var redislock = RedisDistributedSynchronizationProvider.CreateLock($"PRODUCT_{id}");
            using (await redislock.AcquireAsync())
            {
                var product = await Context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();

                b.ProductId = id;
                b.IsPaid = false;

                await Context.Buyers.AddAsync(b);

                if (product.Count > 0)
                {
                    product.Count--;
                    b.IsPaid = true;
                }

                await Context.SaveChangesAsync();
            }

            if (b.IsPaid)
            {
                return true;
            }

            return false;
        }
    }
}