using Microsoft.EntityFrameworkCore;

namespace OtomotoSimpleBackend.Entities
{
    public class OtomotoContext : DbContext
    {
        public OtomotoContext(DbContextOptions<OtomotoContext> options) : base(options)
        {

        }

        public DbSet<Offer> Offers { get; set; }
        public DbSet<Owner> Owners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Offer>()
                .HasKey(o => o.Id);

            modelBuilder.Entity<Owner>()
                .HasKey(o => o.Id);

            modelBuilder.Entity<Offer>()
                .HasOne(o => o.Owner)
                .WithMany(o => o.Offers)
                .HasForeignKey(o => o.OwnerId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
