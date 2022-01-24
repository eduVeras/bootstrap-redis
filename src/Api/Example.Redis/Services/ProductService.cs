using Example.Redis.Interfaces;
using Example.Redis.Models;

namespace Example.Redis.Services
{
    public class ProductService : IProductService
    {
        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
