using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuePOS.API.Data;
using QuePOS.API.Interfaces;
using QuePOS.API.Models;
using QuePOS.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAuthorizationBuilder();
// Replace 'YourDbContext' with the name of your own DbContext derived class.
builder.Services.AddDbContext<POSDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));
    option.EnableDetailedErrors();
});

//service injectioons
builder.Services.AddTransient<SeedService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
//ApplicationUser, IdentityRole
builder.Services
    .AddIdentityApiEndpoints<ApplicationUser>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<POSDbContext>()
    .AddApiEndpoints();
//builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseCors("AllowAngularOrigins");
// Configure the HTTP request pipeline.
app.MapIdentityApi<ApplicationUser>();





//Initialize roles
using (var scope = app.Services.CreateScope())
{
    var roleSeeder = scope.ServiceProvider.GetRequiredService<SeedService>();
    await roleSeeder.SeedAsync();
}

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
