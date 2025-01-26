using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityDemo.Models;

public class CreateRoleModel
{
    [Required] [Display(Name = "Role")] 
    public string RoleName { get; set; }
    public string? Description { get; set; }
}
