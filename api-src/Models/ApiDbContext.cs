using api_src.Maps;
using Microsoft.EntityFrameworkCore;
using api_src.Models;
using System;

namespace api_src.Models{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
       : base(options)
        {

        }
        public DbSet<Company> Companies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new CompanyMap(modelBuilder.Entity<Company>());
        }
    }
}