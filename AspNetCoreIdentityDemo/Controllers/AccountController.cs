using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreIdentityDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;


namespace AspNetCoreIdentityDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IConfiguration _configuration;

    //userManager will hold the UserManager instance
    private readonly UserManager<ApplicationUser> userManager;

    //signInManager will hold the SignInManager instance
    private readonly SignInManager<ApplicationUser> signInManager;

    //Both UserManager and SignInManager services are injected into the AccountController
    //using constructor injection
    public AccountController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        _configuration = configuration;
    }


    [HttpPost]
    [AllowAnonymous]
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

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return UnprocessableEntity(ModelState);
        }

        return UnprocessableEntity(ModelState);
    }

    [HttpPost]
    [AllowAnonymous]
    // login and keep the session active
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
                lockoutOnFailure: false);
            if (result.Succeeded)
            {
                // Handle successful login
                
                // we can assign roles 
            }

            if (result.RequiresTwoFactor)
            {
                // Handle two-factor authentication case
            }

            if (result.IsLockedOut)
            {
                // Handle lockout scenario
            }
            else
            {
                // Handle failure
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return UnprocessableEntity(ModelState);
            }
        }

        return UnprocessableEntity(model);
    }


    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        if (User.Identity.IsAuthenticated)
        {
            await signInManager.SignOutAsync();
            return Ok("User logged out successfully");
        }

        // User is not logged in
        return BadRequest("No user is currently logged in.");
    }

    // login with jwt token 
    [HttpPost("LoginJwt")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginJwt(LoginModel model)
    {
        var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
            lockoutOnFailure: false);
        if (result.Succeeded)
        {
            // Create claims for the user
            var claims = new[]
            {
                // new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.Role, "Admin") // Add roles if needed
            };

            // Get JWT settings from appsettings.json
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the JWT token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
                signingCredentials: creds
            );

            // Return the token as a response
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        // Return unauthorized if credentials are invalid
        return Unauthorized();
    }
}
