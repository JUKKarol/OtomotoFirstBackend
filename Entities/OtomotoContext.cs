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
                .HasKey(o => o.Id); // Ustawienie klucza głównego dla tabeli Offer

            modelBuilder.Entity<Owner>()
                .HasKey(o => o.Id); // Ustawienie klucza głównego dla tabeli Owner

            modelBuilder.Entity<Offer>()
                .HasOne(o => o.Owner) // Definicja relacji wiele-do-jeden
                .WithMany(o => o.Offers) // Definicja relacji jeden-do-wielu
                .HasForeignKey(o => o.OwnerId); // Klucz obcy OwnerId w tabeli Offer

            base.OnModelCreating(modelBuilder);
        }

    }
}
