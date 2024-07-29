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

    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User> _users;

        public UserController(MongoDbService mongoDbService)
        {
            _users = mongoDbService.GetDatabase.GetCollection<User>("User");
        }


        [HttpPost]
        public async Task<ActionResult> Post(User user)
        {
            try
            {
                await _users.InsertOneAsync(user);

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            try
            {
                return Ok(await _users.Find(FilterDefinition<User>.Empty).ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(string id)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(x => x.Id, id);
                var search = await _users.Find(filter).ToListAsync();

                return Ok(search.First());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPut]
        public async Task<ActionResult<User>> Put(User user)
        {
            try
            {
                var update = await _users.ReplaceOneAsync(x => x.Id == user.Id, user);

                return Ok("Usuario foi atualizado com sucesso !!!");
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
                var delete = await _users.DeleteOneAsync(x => x.Id == id);

                if (delete.DeletedCount == 0)
                {
                    return NotFound("Nenhum usuario foi encontrado");
                }

                return Ok("Usuario deletado com sucesso !!!");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
