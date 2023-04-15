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

            // Relacja jeden do wielu (1:N) między tabelami Offer i Owner
            modelBuilder.Entity<Offer>()
                .HasOne(o => o.Owner) // Oferta ma jednego właściciela
                .WithMany(o => o.Offers) // Właściciel ma wiele ofert
                .HasForeignKey(o => o.OwnerId); // Klucz obcy w tabeli Offer

            base.OnModelCreating(modelBuilder);
        }

    }
}
