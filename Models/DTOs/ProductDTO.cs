
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class ProductDTO
{

    public int ProductId { get; set; }
    
    [Required]
    public required string Name { get; set; }
    
    [Required]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Stock must be at least 0.")]
    public int Stock { get; set; }
}
