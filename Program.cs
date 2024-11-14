global using Pacifica.API.Models;
global using Pacifica.API.Helper;
global using Pacifica.API.Data;
global using AutoMapper;

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pacifica.API.Mapper;
using Pacifica.API.Services.BranchProductService;
using Pacifica.API.Services.BranchService;
using Pacifica.API.Services.CategoryService;
using Pacifica.API.Services.ProductService;
using Pacifica.API.Services.SupplierService;
using Pacifica.API.Services.TransactionReferenceService;
using Pacifica.API.Services.AuthService;
using Pacifica.API.Services.EmployeeService;
using Pacifica.API.Services.RoleService;
using Pacifica.API.Services.TokenService;
using Pacifica.API.Services.TransactionTypeService;
using Pacifica.API.Services.ProductStatusService;
using Pacifica.API.Services.StockTransactionServiceInOut;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(); // **This is the fix**



// Adding the DbContext to the service container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Registering Identity services for Employee
builder.Services.AddIdentity<Employee, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Register AutoMapper for DTOs mapping
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register Application services (like Employee, Role, and Auth Services)
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ITransactionReferenceService, TransactionReferenceService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductStatusService, ProductStatusService>();
builder.Services.AddScoped<ITransactionTypeService, TransactionTypeService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IBranchProductService, BranchProductService>();
builder.Services.AddScoped<IStockTransactionServiceInOut, StockTransactionServiceInOut>();


// Adding JWT Authentication (if you plan to use JWT tokens for Authentication)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
    };
});

// Add Authorization service to the DI container
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add middleware for Authentication & Authorization (if using JWT)
app.UseAuthentication();
app.UseAuthorization(); // **Ensure this is present**

app.MapControllers();

app.Run();
