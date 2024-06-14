
using GeekShopping.Email.Context;
using GeekShopping.Email.RabbitMQMessageConsumer;
using GeekShopping.Email.Repositories;
using GeekShopping.Email.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace GeekShopping.Email
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            // Configuração do DbContext
            builder.Services.AddDbContext<SystemDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });



            // Configuração do repositório como Singleton
            var dbContextBuilder = new DbContextOptionsBuilder<SystemDbContext>();
            dbContextBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            builder.Services.AddSingleton(new EmailRepository(dbContextBuilder.Options));

            builder.Services.AddScoped<IEmailRepository, EmailRepository>();

            builder.Services.AddHostedService<RabbitMQPaymentConsumer>();



            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200", "https://localhost:7128", "https://localhost:7201",
                                "https://localhost:7063", "https://localhost:7170", "https://localhost:7109", "https://localhost:7225", "https://localhost:7206")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });


            // Configure JWT authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:7128"; // URL of the authentication service
                options.Audience = "product_api"; // Audience of the API
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SecretKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("ClientOnly", policy => policy.RequireRole("Client", "Admin"));
            });



            // Configurações Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                // Configuração de autenticação do Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Bearer JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "GeekShoppingEmail",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Joao Silveira",
                        Email = "joaoadsistemas@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/this-joao/")
                    }
                });
            });





            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowSpecificOrigin");


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
