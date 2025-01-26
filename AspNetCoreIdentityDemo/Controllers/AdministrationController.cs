using AspNetCoreIdentityDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdministrationController : ControllerBase
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AdministrationController(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    [HttpPost]
    [Route("CreateRole")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
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
            return BadRequest(ModelState);
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
}
