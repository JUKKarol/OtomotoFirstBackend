using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OtomotoSimpleBackend.Data;
using OtomotoSimpleBackend.Data.Seeders;
using OtomotoSimpleBackend.Services;
using OtomotoSimpleBackend.Utilities.Mappings;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
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
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            builder.Services.AddAuthentication().AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            builder.Configuration.GetSection("AppSettings:Token").Value!))
                };
            });

            builder.Services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddDbContext<OtomotoContext>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new OwnerMappingProfile(provider.CreateScope().ServiceProvider.GetService<IOwnerService>()));
                cfg.AddProfile<OfferMappingProfile>();
            }).CreateMapper());

            builder.Services.AddScoped<IOwnerService, OwnerService>();

            var app = builder.Build();
            var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<OtomotoContext>();
            var ownerService = scope.ServiceProvider.GetService<IOwnerService>();

            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                dbContext.Database.Migrate();
            }

            var seeder = new Seeder(dbContext, ownerService);
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