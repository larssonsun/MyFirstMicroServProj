using Microsoft.EntityFrameworkCore;

namespace Service.Consign
{
    public class ConsignContext : DbContext
    {
        public ConsignContext(DbContextOptions<ConsignContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<Consign>();
            e.HasIndex(e => e.ConsignNo).IsUnique();
            
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Consign> Consigns { get; set; }
    }
}