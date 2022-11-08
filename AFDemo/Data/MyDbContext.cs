using AFDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AFDemo.Data
{
    public class MyDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public MyDbContext(DbContextOptions<MyDbContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
        }

        public virtual DbSet<Order> Orders { get; set; }

        protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            SQLitePCL.Batteries.Init();
            await Database.MigrateAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}