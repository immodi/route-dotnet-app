using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public class Order : IEntity
{
    [Key]
    [JsonIgnore]
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Range(0, int.MaxValue, ErrorMessage = "TotalAmount must be between 0 and 100.")]
    public decimal TotalAmount { get; set; }

    [Required]
    public PaymentMethods PaymentMethod { get; set; }
    
    [Required]
    public string Status { get; set; }
    
    [BindNever]
    public virtual Customer Customer { get; set; }

    [BindNever]
    public virtual ICollection<OrderItem>? OrderItems { get; set; }

    [BindNever]
    public virtual Invoice? Invoice { get; set; }
}

public enum PaymentMethods
{
    Cash,
    Paypal,
    Card
}
