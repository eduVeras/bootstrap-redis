using Example.Redis.Interfaces;
using Example.Redis.Models;
using Microsoft.AspNetCore.Mvc;


namespace Example.Redis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IRedisService _redisService;
        private const string Key = "produto";

        public ProductController(IRedisService redisService)
        {
            _redisService = redisService;
        }


        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _redisService.ListAsync<Product>(string.Empty,Key).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {   
            await _redisService.AddOnListAsync(Key,product).ConfigureAwait(false);

            return Created("", new
            {
                success = true
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Product product)
        {
            await _redisService.RemoveAsync(Key, product).ConfigureAwait(false);

            return Ok();
        }
    }
}
