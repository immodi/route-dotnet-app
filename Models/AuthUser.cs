using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class AuthUser : IdentityUser
{    
    public Role Role { get; set; }
}