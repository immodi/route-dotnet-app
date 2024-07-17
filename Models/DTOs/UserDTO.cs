using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class UserDTO
{
    [Required]
    public required string Username { get; set; }
    
    [Required]
    public required string Password { get; set; }

    [Required]
    public Role Role { get; set; }
}