using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityDemo.Models;

public class ApplicationRole : IdentityRole
{
    // added a description property to the IdentityRole
    public string? Description { get; set; }
}
