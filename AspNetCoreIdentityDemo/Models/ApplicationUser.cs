using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityDemo.Models;

public class ApplicationUser : IdentityUser
{
    // add additional properties to the user
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
}
