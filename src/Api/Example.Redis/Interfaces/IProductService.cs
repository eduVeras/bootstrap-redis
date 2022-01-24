using Example.Redis.Models;

namespace Example.Redis.Interfaces
{
    public interface IProductService
    {

        Task<bool> InsertAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(Guid id);    
    }
}
