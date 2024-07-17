using System.Security.Claims;

public interface IAuthService
{
    // string GenerateJwtToken(User user);
    string GenerateToken(IEnumerable<Claim> claims);
    Task<bool> IsAuthenticated(UserDTO userDTO);
    Task<User?> GetByUsername(string userName);

    Task<User?> GetById(int id);

    Task<User?> RegisterUser(UserDTO model);

    Task<bool> DoesUserExist(string userName);

}