using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public class OrderDTO
{

    [BindNever]
    public int OrderId { get; set; }
    
    [Required]
    public int CustomerId { get; set; }

    [BindNever]
    public virtual int TotalAmount { get; set; }

    [BindNever]
    public virtual string Status { get; set; }

    [Required]
    public required PaymentMethods PaymentMethod { get; set; }
    
    [Required]
    public virtual ICollection<OrderItemDTO> OrderItems { get; set; } = null!;
}
