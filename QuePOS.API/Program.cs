using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using QuePOS.API.Data;
using QuePOS.API.Interfaces;
using QuePOS.API.Models;
using QuePOS.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAuthorizationBuilder();
//builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
// Replace 'YourDbContext' with the name of your own DbContext derived class.
builder.Services.AddDbContext<POSDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));
    option.EnableDetailedErrors();
    option.EnableSensitiveDataLogging(true);
});

//service injectioons
builder.Services.AddTransient<CloudinaryService>();
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
//builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "QuePOS API",
        Version = "v1"
    });

    // Add security definition for Bearer Token
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Enter 'Bearer {your_token}' to authenticate",
    });

    // Add global security requirement
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
            new List<string>()
        }
    });
});
builder.Services.AddMailKit(optionBuilder =>
{
    optionBuilder.UseMailKit(new MailKitOptions()
    {

        Server = "smtp.gmail.com",
        Port = 25,
        SenderName = "QUE POS",
        SenderEmail = "noreply",
        Account = builder.Configuration["EmailConfigs:email"],
        Password = builder.Configuration["EmailConfigs:password"],
        Security = true
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
var app = builder.Build();
app.UseCors("AllowAngularOrigins");
// Configure the HTTP request pipeline.


app.MapIdentityApi<ApplicationUser>();





//Initialize roles
using (var scope = app.Services.CreateScope())
{
    var roleSeeder = scope.ServiceProvider.GetRequiredService<SeedService>();
    await roleSeeder.SeedAsync();
    await roleSeeder.SeedAdmin();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
