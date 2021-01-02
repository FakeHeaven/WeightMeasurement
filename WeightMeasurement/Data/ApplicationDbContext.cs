using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WeightMeasurement.Data.Entities;
using WeightMeasurement.Models;

namespace WeightMeasurement.Data
{   
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<SubUser> SubUsers { get; set; }

        public DbSet<SubUserWeight> SubUserWeights { get; set; }

        public DbSet<UserToken> UserTokens { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


    }
}
