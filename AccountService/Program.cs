using AccountService.Data;
using AccountService.Handlers.Address;
using AccountService.Handlers.Authentication;
using AccountService.Handlers.Links;
using AccountService.Handlers.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173", "https://localhost:5173", "https://frontend-uk4w.onrender.com", "http://frontend-uk4w.onrender.com") // React dev server
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddDbContext<AccountDbContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("HostConnection"))
           );

builder.Services.AddScoped<IAddressHandler, AddressHandler>();
builder.Services.AddScoped<IUserHandler, UserHandler>();
builder.Services.AddScoped<IAuthenticationHandler, AuthenticationHandler>();
builder.Services.AddScoped<IOrderLink, OrderLink>();
builder.Services.AddScoped<IReservationLink, ReservationLink>();


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

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AccountDbContext>();
    await DbSeeder.SeedAdminAsync(context);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowFrontend");

app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 200;
        await context.Response.CompleteAsync();
        return;
    }
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/ping", () => "pong");

app.MapGet("/health/db", async (AccountDbContext db) =>
{
    try
    {
        await db.Database.OpenConnectionAsync();
        await db.Database.CloseConnectionAsync();

        return Results.Ok(new { database = true });
    }
    catch (Exception ex)
    {
        return Results.Ok(new
        {
            database = false,
            error = ex.Message,
            full = ex.ToString()
        });
    }
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
