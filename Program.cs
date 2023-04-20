using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using OtomotoSimpleBackend.Data;
using OtomotoSimpleBackend.Data.Seeders;
using System.Text.Json.Serialization;

namespace OtomotoSimpleBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddDbContext<OtomotoContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("OtomotoConnectionString")));

            builder.Services.AddAutoMapper(typeof(Program));

            var app = builder.Build();

            var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetService<OtomotoContext>();

            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                dbContext.Database.Migrate();
            }

            var seeder = new Seeder(dbContext);
            seeder.Seed();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}