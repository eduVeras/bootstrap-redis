using Example.Redis.Interfaces;
using Example.Redis.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Example.Redis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> _logger;
        private readonly IRedisService _redisService;
        private const string Key = "produto";

        public ProductController(ILogger<ProductController> logger, IRedisService redisService)
        {
            _logger = logger;
            _redisService = redisService;
        }


        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _redisService.ListAsync<Product>(string.Empty,Key).ConfigureAwait(false);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {   
            await _redisService.AddOnListAsync(Key,product).ConfigureAwait(false);

            return Created("", new
            {
                success = true
            });
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Product value)
        {
            
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
