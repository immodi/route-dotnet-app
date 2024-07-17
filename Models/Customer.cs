using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public class Customer : IEntity
{
    [JsonIgnore]
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [EmailAddress]
    [Required]
    public string Email { get; set; }
    public virtual ICollection<Order> Orders { get; } = [];
}
