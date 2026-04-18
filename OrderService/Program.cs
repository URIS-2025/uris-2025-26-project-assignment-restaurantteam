using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderService.Data;
using OrderService.Handlers.Links;
using OrderService.Handlers.Order;
using OrderService.Handlers.OrderItem;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173", "https://frontend-uk4w.onrender.com", "http://frontend-uk4w.onrender.com") // React dev server
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<OrderDbContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("HostConnection"))
           );

builder.Services.AddScoped<IOrderHandler, OrderHandler>();
builder.Services.AddScoped<IUserLink, UserLink>();

builder.Services.AddScoped<IMenuItemLink, MenuItemLink>();
builder.Services.AddScoped<IOrderItemHandler, OrderItemHandler>();

builder.Services.AddHttpClient();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)
        ),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        //RoleClaimType = "role",
        NameClaimType = "unique_name"
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("JWT Authentication Failed: " + context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("JWT Token Validated: " + context.SecurityToken);
            return Task.CompletedTask;
        },
        OnMessageReceived = context =>
        {
            Console.WriteLine("JWT Received: " + context.Request.Headers["Authorization"]);
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
