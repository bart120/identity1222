using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.AspIdentity
{
    public class AuthenticationDbContext : IdentityDbContext<User>
    {

        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("auth");
        }
    }
}
