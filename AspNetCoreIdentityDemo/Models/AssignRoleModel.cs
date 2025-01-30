namespace AspNetCoreIdentityDemo.Models;


public class AssignRoleModel
{
    public string UserId { get; set; } // User ID or username/email
    public string RoleName { get; set; } // Role to assign
}
