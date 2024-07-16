using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SqlServerWebApi.Data;
using SqlServerWebApi.Models;
using AutoMapper;


var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

// Register the configuration (already provided by default in the WebApplicationBuilder)
var configuration = builder.Configuration;

builder.Services.AddScoped(typeof(IRepository<User>), typeof(Repository<User>));
builder.Services.AddScoped(typeof(IRepository<Customer>), typeof(Repository<Customer>));
builder.Services.AddScoped(typeof(IRepository<Invoice>), typeof(Repository<Invoice>));
builder.Services.AddScoped(typeof(IRepository<Product>), typeof(Repository<Product>));
builder.Services.AddScoped(typeof(IRepository<Order>), typeof(Repository<Order>));
builder.Services.AddScoped(typeof(IRepository<OrderItem>), typeof(Repository<OrderItem>));
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Register CustomerRepository with the configuration

builder.Services.AddDbContext<OrderManagementDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
