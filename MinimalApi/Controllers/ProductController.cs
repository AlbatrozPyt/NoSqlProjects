using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Domains;
using MinimalApi.Services;
using MongoDB.Driver;
using System.ComponentModel;

namespace MinimalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        private readonly IMongoCollection<Product>? _product;

        public ProductController(MongoDbService mongoDbService)
        {
            _product = mongoDbService.GetDatabase.GetCollection<Product>("Product");
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            try
            {
                var products = await _product.Find(FilterDefinition<Product>.Empty).ToListAsync();

                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(string id)
        {
            try
            {

                // Forma simples
                // var product = await _product.Find("lambda").FirstOrDefautAsync()

                // Cria um filtro para a busca
                var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
                var res = await _product.Find(filter).ToListAsync();

                if (res != null)
                {
                    return Ok(res.First());
                }

                return NotFound();

            }
            catch (Exception)
            {

                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post(Product product)
        {
            try
            {
                Product product1 = new Product();
                product1.Name = product.Name;
                product1.Price = product.Price;
                product1.AdditionalAttributes = product.AdditionalAttributes;

                _product.InsertOne(product1);

                return Ok(product1);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var res = await _product.DeleteOneAsync(x => x.Id == id);

                if (res.DeletedCount == 1)
                {
                    return Ok("O produto foi encontrado");
                }

                return NotFound("Nenhum produto encontrado");

            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPut]
        public async Task<ActionResult<Product>> Update(string id, Product product)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
                var res = await _product.Find(filter).ToListAsync();

                if (product.Name == null)
                {
                    product.Name = res.First().Name;
                }

                if (product.Price == 0)
                {
                    product.Price = res.First().Price;
                }

                if (product.AdditionalAttributes == null)
                {
                    product.AdditionalAttributes = res.First().AdditionalAttributes;
                }

                var update = Builders<Product>.Update
                    .Set(x => x.Name, product.Name)
                    .Set(x => x.Price, product.Price)
                    .Set(x => x.AdditionalAttributes, product.AdditionalAttributes);

                await _product.UpdateOneAsync(filter, update);

                return Ok();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

    }
}
