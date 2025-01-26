using AspNetCoreIdentityDemo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentityDemo;

public class
    ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string> // application user extends IdentityUser , application role extends IdentityRole

{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // add the tables under the Identity schema
        // modelBuilder.HasDefaultSchema("Identity");
    }
}
