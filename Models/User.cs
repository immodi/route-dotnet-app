using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class User : IEntity
{
    [JsonIgnore]
    [Key]
    public int Id { get; set; }

    [Required]
    public required string Username { get; set; }
    
    [Required]
    public required string PasswordHash { get; set; }
    
    [RegularExpression("^(admin|customer)$", ErrorMessage = "Role must be either 'admin' or 'customer'.")]
    [Required]
    public required string Role { get; set; }
}