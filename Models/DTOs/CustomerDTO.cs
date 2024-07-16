using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public class CustomerDTO
{
    public int CustomerId { get; set; }

    [Required]
    public required string Name { get; set; }

    [EmailAddress]
    [Required]
    public required string Email { get; set; }
    public virtual ICollection<OrderDTO> Orders { get; } = [];
}
