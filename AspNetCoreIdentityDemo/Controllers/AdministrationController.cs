using AspNetCoreIdentityDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdministrationController : ControllerBase
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdministrationController(RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost]
    [Route("CreateRole")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleModel model)
    {
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }

        bool roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
        if (roleExists)
        {
            return BadRequest("Role already exists");
        }

        var identityRole = new ApplicationRole
        {
            Name = model.RoleName
        };
        IdentityResult result = await _roleManager.CreateAsync(identityRole);
        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("EditRole")]
    public async Task<IActionResult> EditRole(UpdateRoleModel model)
    {
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }

        var role = await _roleManager.FindByIdAsync(model.Id);
        if (role == null)
        {
            return BadRequest($"Role with Id = {model.Id} cannot be found");
        }


        role.Name = model.RoleName;
        role.Description = model.Description;
        // Update other properties if needed

        var result = await _roleManager.UpdateAsync(role);
        if (result.Succeeded)
        {
            return NoContent();
        }

        return BadRequest(result.Errors);
    }

    [HttpDelete("DeleteRole/{id}")]
    public async Task<IActionResult> DeleteRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            return BadRequest($"Role with Id = {id} cannot be found");
        }

        var result = await _roleManager.DeleteAsync(role);
        if (result.Succeeded)
        {
            return NoContent();
        }

        return BadRequest(result.Errors);
    }


    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        // Check if the role exists
        var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
        if (!roleExists)
        {
            return BadRequest("Role does not exist.");
        }

        // Assign the role to the user
        var result = await _userManager.AddToRoleAsync(user, model.RoleName);
        if (result.Succeeded)
        {
            return Ok($"Role '{model.RoleName}' assigned to user '{user.UserName}' successfully.");
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("remove-role")]
    public async Task<IActionResult> RemoveRole([FromBody] AssignRoleModel model)
    {
        // Find the user
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        // Check if the role exists
        var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
        if (!roleExists)
        {
            return BadRequest("Role does not exist.");
        }

        // Remove the role from the user
        var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
        if (result.Succeeded)
        {
            return Ok($"Role '{model.RoleName}' removed from user '{user.UserName}' successfully.");
        }

        return BadRequest(result.Errors);
    }

    [HttpGet("user-roles/{userId}")]
    public async Task<IActionResult> GetUserRoles(string userId)
    {
        // Find the user
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        // Get the user's roles
        var roles = await _userManager.GetRolesAsync(user);
        return Ok(roles);
    }



}
