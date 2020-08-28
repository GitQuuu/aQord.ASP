using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using aQord.ASP.Services;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace aQord.ASP.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // Get Connectionstring from Keyvault to be used in ApplicationDbContext
        private static KeyVaultSecret secret = KeyVaultService.KeyVaultSecret("QuDevConnectionString", KeyVaultService.AuthenticateCreateClient());

        public DbSet<Person> People  { get; set; } 
        public DbSet<Schematics> Schematics { get; set; }

        public DbSet<Hours> Hours { get; set; }
        public ApplicationDbContext()
            : base($"{secret.Value}", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}