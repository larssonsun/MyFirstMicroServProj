using Microsoft.EntityFrameworkCore;

namespace Service.Sample
{
    public class SampleContext : DbContext
    {
        public SampleContext(DbContextOptions<SampleContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<Sample>();
            e.HasIndex(e => e.SampleNo).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Sample> Samples { get; set; }
    }
}