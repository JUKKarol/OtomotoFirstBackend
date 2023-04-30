using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OtomotoSimpleBackend.Entities;

namespace OtomotoSimpleBackend.Data
{
    public class OtomotoContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public OtomotoContext(DbContextOptions<OtomotoContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Offer> Offers { get; set; }
        public DbSet<Owner> Owners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Offer>(eb =>
            {
                eb.HasKey(o => o.Id);
                eb.HasOne(o => o.Owner)
                .WithMany(o => o.Offers)
                .HasForeignKey(o => o.OwnerId);

                eb.Property(o => o.Brand)
                .IsRequired()
                .HasMaxLength(20);

                eb.Property(o => o.Model)
                .IsRequired()
                .HasMaxLength(20);

                eb.Property(o => o.Body)
                .HasMaxLength(20);

                eb.Property(o => o.FuelType)
                .HasMaxLength(15);

                eb.Property(o => o.EngineSizeInL)
                .HasPrecision(5, 3);
            });

            modelBuilder.Entity<Owner>(eb =>
            {
                eb.HasKey(o => o.Id);

                eb.Property(o => o.FirstName)
                .IsRequired()
                .HasMaxLength(20);

                eb.Property(o => o.LastName)
                .IsRequired()
                .HasMaxLength(20);

                eb.Property(o => o.PhoneNumber)
               .IsRequired();

                eb.Property(o => o.City)
               .HasMaxLength(25);

                eb.Property(o => o.Email)
               .IsRequired()
               .HasMaxLength(30);
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseSqlServer(_configuration.GetConnectionString("OtomotoConnectionString"));
        }
    }
}
