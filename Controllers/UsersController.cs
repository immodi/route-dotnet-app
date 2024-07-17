using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SqlServerWebApi.Data;

namespace JwtRoleAuthentication.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly IRepository<User> _repository;
    private readonly OrderManagementDbContext _context;
    private readonly TokenService _tokenService;
    private readonly IMapper _mapper;

    public UsersController(UserManager<AuthUser> userManager, OrderManagementDbContext context, TokenService tokenService, IMapper mapper, IRepository<User> repository)
    {
        _userManager = userManager;
        _context = context;
        _tokenService = tokenService;
        _mapper = mapper;
        _repository = repository;
    }

    
    [AllowAnonymous]
    [HttpPost]
    [Route("/[controller]/users/register")]
    public async Task<IActionResult> Register(UserDTO request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var user = new AuthUser { UserName = request.Username, Role = request.Role , Email = request.Username + "@site.com" };
        var result = await _userManager.CreateAsync(
            user,
            request.Password!
        );

        if (result.Succeeded)
        {
            var dbUser = new User {PasswordHash = user.PasswordHash, Role = user.Role.ToString(), Username = user.UserName };
            await _repository.AddAsync(dbUser);
            request.Password = "";
            return CreatedAtAction(nameof(Register), new { userName = request.Username, role = request.Role }, request);
        }

        await _userManager.DeleteAsync(user);
        
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return BadRequest(ModelState);
    }
    
    
    [AllowAnonymous]
    [HttpPost]
    [Route("/[controller]/users/login")]
    public async Task<ActionResult<string>> Authenticate([FromBody] UserDTO request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var managedUser = await _userManager.FindByNameAsync(request.Username!);
        if (managedUser == null)
        {
            return BadRequest("Bad credentials");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password!);
        if (!isPasswordValid)
        {
            return BadRequest("Bad credentials");
        }

        var userInDb = _context.Users.FirstOrDefault(u => u.UserName == request.Username);
        if (userInDb is null)
        {
            return Unauthorized();
        }

        var accessToken = _tokenService.CreateToken(managedUser);
        await _context.SaveChangesAsync();
        
        return Ok(new
        {
            Username = userInDb.UserName,
            Token = accessToken,
        });
    }

    // [HttpGet]
    // [Route("test")]
    // [Authorize (Roles = "Admin")]
    // public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    // {
    //     return Ok(await _repository.GetAllAsync());
    // }


}
