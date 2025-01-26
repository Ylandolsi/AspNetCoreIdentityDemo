using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreIdentityDemo.Models;


namespace AspNetCoreIdentityDemo.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    //userManager will hold the UserManager instance
    private readonly UserManager<ApplicationUser> userManager;

    //signInManager will hold the SignInManager instance
    private readonly SignInManager<ApplicationUser> signInManager;

    //Both UserManager and SignInManager services are injected into the AccountController
    //using constructor injection
    public AccountController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }



    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            // Copy data from RegisterViewModel to ApplicationUser
            var user = new ApplicationUser
            {
                UserName = model.Email,
                LastName = model.LastName,
                FirstName = model.FirstName,
                Email = model.Email
            };

            // Store user data in AspNetUsers database table
            var result = await userManager.CreateAsync(user, model.Password);

            // If user is successfully created, sign-in the user using
            // SignInManager and redirect to index action of HomeController
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return Ok("User Registred Successfully");
            }

            return BadRequest("Invalid Data");
        }

        return BadRequest("Invalid Data");
    }
}
