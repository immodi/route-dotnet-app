using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SqlServerWebApi.Data;
using SqlServerWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlServerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _repository;

        public UserController(IRepository<User> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var Items = await _repository.GetAllAsync();
            return Ok(Items);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Add(User user)
        {
            try
            {
                var User = await _repository.AddAsync(user);
                return Ok(User);    
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    

    }
}
