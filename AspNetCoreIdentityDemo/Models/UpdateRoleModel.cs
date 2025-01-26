using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityDemo.Models;

public class UpdateRoleModel
{
    [Required] public string Id { get; set; }

    [Required(ErrorMessage = "Role Name is Required")]
    public string RoleName { get; set; }

    public string? Description { get; set; }
}
