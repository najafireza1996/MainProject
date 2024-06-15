using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Features.User.Domain.Entities;
using Features.Authentication.Domain.Entities;

namespace Features.Core.Infrastructure.DataAccess
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);



            base.OnModelCreating(builder);
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

    }
}