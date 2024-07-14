using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqlServerWebApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Register the configuration (already provided by default in the WebApplicationBuilder)
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register CustomerRepository with the configuration
string connectionString = configuration.GetConnectionString("DefaultConnection");

builder.Services.AddSingleton(sp => new ItemRepository(connectionString));

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
