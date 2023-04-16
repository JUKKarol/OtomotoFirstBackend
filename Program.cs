
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using OtomotoSimpleBackend.Entities;
using System.Text.Json.Serialization;

namespace OtomotoSimpleBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });


            builder.Services.AddDbContext<OtomotoContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("OtomotoConnectionString")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //Informacja przed uruchomieniem api czy s¹ jakieœ nieza³adowane migracje na bazie danych (jeœli s¹to sie zaaplikuj¹)
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<OtomotoContext>();

            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                dbContext.Database.Migrate();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}