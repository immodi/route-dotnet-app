
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public class Invoice : IEntity
{
    [JsonIgnore]
    [Key]
    public int Id { get; set; }
    public int OrderId { get; set; }
    public DateTime InvoiceDate { get; set; } = DateTime.Now;

    [Range(0, int.MaxValue, ErrorMessage = "TotalAmount must be between 0 and 100.")]
    public decimal TotalAmount { get; set; }

    public virtual Order Order { get; set; } = null!;
}