using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SqlServerWebApi.Data;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;


var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var validIssuer = builder.Configuration.GetValue<string>("JwtTokenSettings:ValidIssuer");
var validAudience = builder.Configuration.GetValue<string>("JwtTokenSettings:ValidAudience");
var symmetricSecurityKey = builder.Configuration.GetValue<string>("JwtTokenSettings:SymmetricSecurityKey");

builder.Services.AddScoped(typeof(IRepository<User>), typeof(Repository<User>));
builder.Services.AddScoped(typeof(IRepository<Customer>), typeof(Repository<Customer>));
builder.Services.AddScoped(typeof(IRepository<Invoice>), typeof(Repository<Invoice>));
builder.Services.AddScoped(typeof(IRepository<Product>), typeof(Repository<Product>));
builder.Services.AddScoped(typeof(IRepository<Order>), typeof(Repository<Order>));
builder.Services.AddScoped(typeof(IRepository<OrderItem>), typeof(Repository<OrderItem>));
builder.Services.AddScoped<TokenService, TokenService>();
// builder.Services.AddScoped(typeof(UserManager<>));

// builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));


builder.Services.AddSingleton(configuration);
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddProblemDetails();
builder.Services.AddRouting(options => options.LowercaseUrls = true);


builder.Services.AddDbContext<OrderManagementDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services
.AddIdentity<AuthUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<OrderManagementDbContext>();


builder.Services.AddControllers(
    config => config.Filters.Add(new ApiResultFilter())).AddJsonOptions(opt => {
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// builder.Services.AddAuthentication(options => {
//         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//         options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//     }).AddJwtBearer(options =>
// {
//         options.SaveToken = true;
//         options.RequireHttpsMetadata = true;
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidIssuer = "https://www.you.com",
//             ValidAudiences = ["https://you.com", "https://www.you.com"],
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey))
//         };
        
//     });

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;})
.AddJwtBearer(options =>
{
    options.IncludeErrorDetails = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ClockSkew = TimeSpan.Zero,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = validIssuer,
        ValidAudience = validAudience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(symmetricSecurityKey)
        ),
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", 
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});
    
var app = builder.Build();


// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStatusCodePages();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();