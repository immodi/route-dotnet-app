using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public class OrderStausDTO
{    
    [Required]
    public string Status { get; set; }
}
