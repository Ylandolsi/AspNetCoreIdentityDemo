using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityDemo.Configurations;

public class CustomPasswordValidator : IPasswordValidator<IdentityUser>
{
    public Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user, string password)
    {
        // Check if the password contains at least one alphabetic character
        if (!password.Any(char.IsLetter))
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Code = "PasswordRequiresLetter",
                Description = "The password must contain at least one alphabetic character."
            }));
        }

        return Task.FromResult(IdentityResult.Success);
    }
}
