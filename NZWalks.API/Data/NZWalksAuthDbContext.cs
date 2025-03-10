using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data;

public class NZWalksAuthDbContext : IdentityDbContext
{
    public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var readerRoleId = "06929778-68a2-426c-b085-9597ae6b9493";
        var writerRoleId = "338471f8-b0a7-4dc7-8e8c-498fda0472f7";
        var roles = new List<IdentityRole>
         {
             new IdentityRole
             {
                 Id = readerRoleId,
                 ConcurrencyStamp=readerRoleId,
                 Name = "Reader",
                 NormalizedName = "Reader".ToUpper(),
             },
              new IdentityRole
             {
                 Id = writerRoleId,
                 ConcurrencyStamp=writerRoleId,
                 Name = "Writer",
                 NormalizedName = "Writer".ToUpper(),
             },

        };

        builder.Entity<IdentityRole>().HasData(roles);
    }
}
