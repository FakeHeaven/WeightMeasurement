using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WeightMeasurement.Data.Entities;

namespace WeightMeasurement.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<SubUser> SubUsers { get; set; }

        public DbSet<SubUserWeight> SubUserWeights { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
