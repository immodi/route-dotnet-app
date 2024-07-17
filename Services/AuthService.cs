// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.IdentityModel.Tokens;
// using BC = BCrypt.Net.BCrypt;

// public class AuthService(
//         IConfiguration config,
//         IRepository<User> userRepository,
//         UserManager<IUser> userManager,
//         RoleManager<IdentityRole> roleManager,
//         IConfiguration configuration
//     ) : IAuthService
// {
//     private readonly IConfiguration _config = config;
//     private readonly IRepository<User> _userRepository = userRepository;
//     private readonly UserManager<IUser> _userManager = userManager;
//     private readonly RoleManager<IdentityRole> _roleManager = roleManager;
//     private readonly IConfiguration _configuration = configuration;


//     public string GenerateJwtToken(User user) 
//     {
//         var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("JWT_Secret")));
//         var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

//         var claims = new[]
//         {
//             new Claim(JwtRegisteredClaimNames.Sub, user.Username),
//             new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
//             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//         };


//         var token = new JwtSecurityToken(
//             issuer: "https://www.you.com",
//             audience: "https://www.you.com",
//             claims: claims,
//             expires: DateTime.Now.AddDays(1),
//             signingCredentials: credentials
//         );

//         return new JwtSecurityTokenHandler().WriteToken(token);
//     }

//     public async Task<bool> IsAuthenticated(UserDTO userDTO)
//     {
//         var user = await GetByUsername(userDTO.Username);
//         if (user == null) return false;
        
//         return BC.Verify(userDTO.Password, user.PasswordHash);
//     }

//     public async Task<User?> GetById(int id)
//     {
//         return await _userRepository.GetByIdAsync(id);
//     }

//     public async Task<User?> GetByUsername(string userName)
//     {
//         var users = await _userRepository.GetAllAsync();
//         var user = users.ToList().Find(user => user.Username == userName);
//         return user;
//     }


//     public async Task<bool> DoesUserExist(string userName)
//     {
//         var users = await _userRepository.GetAllAsync();

//         var user = users.FirstOrDefault(user => user.Username == userName);
//         return user != null;
        
//     }

//     public async Task<User?> RegisterUser(UserDTO model)
//     {
//         var passwordHash = BC.HashPassword(model.Password);

//         var iUser = new IUser {
//             UserName = model.Username,
//             Role = model.Role,
//             PasswordHash = passwordHash
//         };
        
//         var createUserResult = await _userManager.CreateAsync(iUser);
//         if (!createUserResult.Succeeded)
//         {
//             System.Console.WriteLine("error when user manager");
//             return null;
//         }

            
//         if (iUser.Role != Roles.Admin | iUser.Role != Roles.Customer) return null; System.Console.WriteLine("not admin or customer");;

//         if (!await _roleManager.RoleExistsAsync(iUser.Role)) 
//         {
//             await _roleManager.CreateAsync(new IdentityRole(iUser.Role));
//         }
        
//         await _userManager.AddToRoleAsync(iUser, iUser.Role);

//         var user = new User{
//             Role = model.Role,
//             Username = model.Username,
//             PasswordHash = passwordHash
//         };

//         var userEntity = await _userRepository.AddAsync(user);
//         if (userEntity == null)
//         {
//             System.Console.WriteLine("error when makign db user");
//         }

//         return userEntity;
//     }

//     public string GenerateToken(IEnumerable<Claim> claims)
//     {
//         var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("JWT_Secret")));

//         var tokenDescriptor = new SecurityTokenDescriptor
//         {
//             Issuer = "https://www.you.com",
//             Audience = "https://www.you.com",
//             Expires = DateTime.UtcNow.AddHours(3),
//             SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),
//             Subject = new ClaimsIdentity(claims)
//         };

  
//         var tokenHandler = new JwtSecurityTokenHandler();
//         var token = tokenHandler.CreateToken(tokenDescriptor);
//         return tokenHandler.WriteToken(token);
//     }
// }