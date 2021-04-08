using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repo;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository repo, ILogger<CatalogController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _repo.GetProducts());
        }

        [HttpGet("{id:length(24)}",Name = "GetProduct")]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductbyId(string id)
        {
            var result = await _repo.GetProduct(id);
            if(result == null)
            {
                _logger.LogError($"Product with id: {id} not found");
                return NotFound();
            }
            return Ok(result);
        }

        [Route("[action]/{category}", Name = "GetProductsbyCategory")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductsbyCategory(string category)
        {
            var result = await _repo.GetProductsByCategory(category);
            if (result == null)
            {
                _logger.LogError($"Product(s) with category: {category} not found");
                return NotFound();
            }
            return Ok(result);
        }

        [Route("[action]/{name}", Name = "GetProductsbyName")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductsbyName(string name)
        {
            var result = await _repo.GetProductsByCategory(name);
            if (result == null)
            {
                _logger.LogError($"Product(s) with name: {name} not found");
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody]Product product)
        {
            await _repo.CreateProduct(product);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct([FromBody] Product product)
        {
            var result = await _repo.UpdateProduct(product);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            var result = await _repo.DeleteProduct(id);
            return Ok(result);
        }
    }
}
