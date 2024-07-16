using Microsoft.AspNetCore.Identity;
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
    public class UsersController : ControllerBase
    {
        private readonly IRepository<User> _repository;
        private readonly RoleManager<IdentityRole> _role_manager;


        public UsersController(IRepository<User> repository, RoleManager<IdentityRole> roleManager)
        {
            _repository = repository;
            _role_manager = roleManager;
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddUser(User user)
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
    
        // [HttpGet]
        // public async

    }
}
