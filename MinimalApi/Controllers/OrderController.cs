using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Domains;
using MinimalApi.Services;
using MinimalApi.ViewModels;
using MongoDB.Driver;

namespace MinimalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrderController : ControllerBase
    {
        private readonly IMongoCollection<Order> _order;
        private readonly IMongoCollection<Client> _client;
        private readonly IMongoCollection<Product> _product;

        public OrderController(MongoDbService mongoDbService)
        {
            _order = mongoDbService.GetDatabase.GetCollection<Order>("Order");
            _client = mongoDbService.GetDatabase.GetCollection<Client>("Client");
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> Get()
        {
            try
            {
                var res = _order.Find(FilterDefinition<Order>.Empty).ToList();
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Post(OrderViewModel order)
        {
            try
            {
                Order order1 = new Order();
                order1.Id = order.Id;
                order1.ProductId = order.ProductId;
                order1.ClientId = order.ClientId;
                order1.Status = order.Status;
                order1.Date = order.Date;

                Client client = await _client.Find(x => x.Id == order.ClientId).FirstOrDefaultAsync();

                if (client == null)
                {
                    return NotFound("O Id do Cliente não é válido");
                }

                order1.Client = client;



                order1.Products = new List<Product>();

                foreach (string id in order1.ProductId!)
                {
                    Product product = await _product.Find(x => x.Id == id).FirstOrDefaultAsync();

                    if (product != null)
                    {
                        order1.Products.Add(product);
                    }

                }

                await _order.InsertOneAsync(order1);

                return StatusCode(201);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(string id)
        {
            try
            {
                var obj = await _order.Find(x => x.Id == id).FirstOrDefaultAsync();

                if (obj == null)
                {
                    return NotFound("O Id do pedido não foi encontrado !!!");
                }

                return Ok(obj);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put(Order order, string orderId)
        {
            try
            {

                var orderExists = await _order.Find(x => x.Id == orderId).FirstOrDefaultAsync();

                if (orderExists != null)
                {

                    Order order1 = await _order.Find(x => x.Id == orderId).FirstOrDefaultAsync();
                    order1.ProductId = order.ProductId != null ? order.ProductId : order1.ProductId;
                    order1.ClientId = order.ClientId != null ? order.ClientId : order1.ClientId;
                    order1.Status = order.Status != null ? order.Status : order1.Status;
                    order1.Date = order.Date != null ? order.Date : order1.Date;

                    if (order.ClientId != null)
                    {
                        Client client = await _client.Find(x => x.Id == order.ClientId).FirstOrDefaultAsync();

                        if (client == null)
                        {
                            return NotFound("O Id do cliente não é válido");
                        }

                        order1.Client = client;
                    }

                    if (order.ProductId != null)
                    {
                        var updateProdutos = new List<Product>();

                        foreach (string id in order.ProductId)
                        {
                            Product product = await _product.Find(x => x.Id == id).FirstOrDefaultAsync();

                            if (product != null)
                            {
                                updateProdutos.Add(product);
                            }
                        }

                        order1.Products = updateProdutos;
                    }


                    await _order.ReplaceOneAsync(x => x.Id == orderId, order1);

                    return Ok("Atualizado com sucesso");
                }

                return NotFound("Não foi encontrado nenhum pedido com esse Id !!!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {

                if ((await _order.Find(x => x.Id == id).FirstOrDefaultAsync()) == null)
                {
                    return NotFound("O Id do pedido não foi encontrado !!!");
                }

                await _order.DeleteOneAsync(x => x.Id == id);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

    }
}
