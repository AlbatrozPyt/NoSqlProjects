using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Domains;
using MinimalApi.Services;
using MongoDB.Driver;

namespace MinimalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ClientController : ControllerBase
    {
        private readonly IMongoCollection<Client>? _client;

        public ClientController(MongoDbService mongoDbService)
        {
            _client = mongoDbService.GetDatabase.GetCollection<Client>("Client");
        }


        [HttpPost]
        public async Task<ActionResult> Post(Client client)
        {
            try
            {
                await _client!.InsertOneAsync(client);

                return Created();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Client>>> Get()
        {
            try
            {
                return Ok(await _client.Find(FilterDefinition<Client>.Empty).ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string id)
        {
            try
            {
                Client obj = await _client.Find(x => x.Id == id).FirstOrDefaultAsync();

                if (obj == null)
                {
                    return NotFound("Nenhum cliente foi encontrado com esse Id !!!"); 
                }

                return Ok(obj);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{clientId}")]
        public async Task<ActionResult> Update(Client client, string clientId)
        {
            try
            {
                Client client1 = await _client.Find(x => x.Id == clientId).FirstOrDefaultAsync();
                client1.Cpf = client.Cpf != null ? client.Cpf : client1.Cpf;
                client1.Phone = client.Phone != null ? client.Phone : client1.Phone;
                client1.Address = client.Address != null ? client.Address : client1.Address;
                client1.AdditionalAtributes = client.AdditionalAtributes != null ? client.AdditionalAtributes : client1.AdditionalAtributes;

                var update = await _client.ReplaceOneAsync(x => x.Id == client.Id, client1);

                return Ok("Cliente atualizado com sucesso");
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
                Client client1 = await _client.Find(x => x.Id == id).FirstOrDefaultAsync();

                if(client1 != null)
                {
                    await _client.DeleteOneAsync(x => x.Id == id);
                    return NoContent();
                }

                return NotFound("Nao foi encontrado nenhum Cliente com esse Id");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
