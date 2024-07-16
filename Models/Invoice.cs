
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
    public decimal TotalAmount { get; set; }

    public virtual Order Order { get; set; } = null!;
}