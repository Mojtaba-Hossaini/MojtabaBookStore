using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MojtabaBookStore.Areas.Identity.Data;

namespace MojtabaBookStore.Models
{
    public class MojtabaIdentityContext : IdentityDbContext<MojtabaBookStoreUser, ApplicationRole,string, IdentityUserClaim<string>,ApplicationUserRole,IdentityUserLogin<string>,IdentityRoleClaim<string>,IdentityUserToken<string>>
    {
        public MojtabaIdentityContext(DbContextOptions<MojtabaIdentityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationRole>().ToTable("AspNetRoles").ToTable("AppRoles");
            builder.Entity<ApplicationUserRole>().ToTable("AppUserRole");
            builder.Entity<ApplicationUserRole>().HasOne(ur => ur.Role).WithMany(r => r.Users).HasForeignKey(f => f.RoleId);
        }
    }
}
