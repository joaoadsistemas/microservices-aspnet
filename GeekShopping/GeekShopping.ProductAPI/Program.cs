using GeekShopping.ProductAPI.Context;
using GeekShopping.ProductAPI.Repositories;
using GeekShopping.ProductAPI.Repositories.Interfaces;
using GeekShopping.ProductAPI.Services;
using GeekShopping.ProductAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// BANCO DE DADOS
builder.Services.AddDbContext<SystemDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();


// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200", "https://localhost:7128", "https://localhost:7201",
                    "https://localhost:7063", "https://localhost:7170", "https://localhost:7109")
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
        Title = "GeekShoppingProduct",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Joao Silveira",
            Email = "joaoadsistemas@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/this-joao/")
        }
    });
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// Enable CORS
app.UseCors("AllowSpecificOrigin");


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
