using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Domain
{
    public class DbContext : IdentityDbContext<IdentityUser>
    {

        public class CALLMANAGERContext : IdentityDbContext<IdentityUser>
        {

            public CALLMANAGERContext(DbContextOptions<DbContext> options) : base(options)
            { }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);

                #region "Seed Data"

                builder.Entity<IdentityRole>().HasData(
                    new { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                    new { Id = "2", Name = "Customer", NormalizedName = "CUSTOMER" }
                );

                

                #endregion
            }

        }

    }
}
