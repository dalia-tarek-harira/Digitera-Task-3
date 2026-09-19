using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Task_3.Application.Interfaces;
using Task_3.Application.Services;
using Task_3.Infrastructure.Data;
using Task_3.Infrastructure.Identity;
using Task_3.Infrastructure.Repositories;

namespace Task_3.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers
            builder.Services.AddControllers();

            // HttpContext
            builder.Services.AddHttpContextAccessor();

            // Database
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString(
                        "DefaultConnection"));
            });

            // Identity
            builder.Services
                .AddIdentityCore<ApplicationUser>(options =>
                {
                    options.User.RequireUniqueEmail = true;

                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                })
                .AddRoles<IdentityRole<int>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // JWT
            var jwtKey =
                builder.Configuration["Jwt:Key"]
                ?? throw new InvalidOperationException(
                    "JWT Key is missing.");

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme =
                        JwtBearerDefaults.AuthenticationScheme;

                    options.DefaultChallengeScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer =
                                builder.Configuration["Jwt:Issuer"],

                            ValidAudience =
                                builder.Configuration["Jwt:Audience"],

                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(jwtKey))
                        };
                });

            builder.Services.AddAuthorization();

            // Repositories
            builder.Services.AddScoped<IJobRepository, JobRepository>();

            builder.Services.AddScoped<
                IJobApplicationRepository,
                JobApplicationRepository>();

            // Current User
            builder.Services.AddScoped<
                ICurrentUserService,
                CurrentUserService>();

            // Authentication
            builder.Services.AddScoped<
                IAuthService,
                AuthService>();

            // Application Services
            builder.Services.AddScoped<ApplicationService>();
            builder.Services.AddScoped<JobService>();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            // Seed Roles
            using (var scope = app.Services.CreateScope())
            {
                var roleManager =
                    scope.ServiceProvider
                        .GetRequiredService<
                            RoleManager<IdentityRole<int>>>();

                await IdentitySeeder.SeedRolesAsync(roleManager);
            }

            app.MapControllers();

            app.Run();
        }
    }
}