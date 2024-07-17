using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

public class User : IEntity 
{
    [JsonIgnore]
    [Key]
    public int Id { get; set; }

    [Required]
    public string Username { get; set; }
    
    [Required]
    public string PasswordHash { get; set; }
    
    [RegularExpression("^(Admin|Customer)$", ErrorMessage = "Role must be either 'Admin' or 'Customer'.")]
    [Required]
    public string Role { get; set; }
}