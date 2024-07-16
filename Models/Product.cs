
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

public class Product : IEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }
    public decimal Price { get; set; }
  
    [Range(0, int.MaxValue, ErrorMessage = "Stock must be at least 0.")]
    public int Stock { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; } = [];
}
