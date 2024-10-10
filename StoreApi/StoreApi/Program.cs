using Microsoft.EntityFrameworkCore;
using StoreApi.Data;
using StoreApi.Data.Entities;
using StoreApi.Mapper;
using StoreApi.Services;
using StoreApi.Interfaces;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using StoreApi.Data.Entities.Identity;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApiStoreDbContext>(opt =>
       opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;


builder.Services.AddSwaggerGen(c =>
{
    var fileDoc = Path.Combine(AppContext.BaseDirectory, $"{assemblyName}.xml");
    c.IncludeXmlComments(fileDoc);
    c.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Description = "Jwt Auth header using the Bearer scheme",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer"
        });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
             new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                }, new List<string>()
        }
    });
});
builder.Services.AddCors();

builder.Services.AddAuthentication();

builder.Services.AddAutoMapper(typeof(AppMapProfile));
builder.Services.AddScoped<IImageTool, ImageTool>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<DataSeeder>();

var jwtKey = builder.Configuration.GetValue<string>("Jwt:Key");

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.RequireHttpsMetadata = false;
    opt.SaveToken = true;
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Identity options
builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

})
    .AddEntityFrameworkStores<ApiStoreDbContext>()
    .AddDefaultTokenProviders();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(opt =>
opt.AllowAnyMethod()
.AllowAnyHeader()
.AllowAnyOrigin());


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), builder.Configuration["ImagesDir"])),
    RequestPath = "/images"
});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApiStoreDbContext>();
    //dbContext.Database.EnsureDeleted();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();

    dbContext.Database.Migrate();

    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedData();
}



app.Run();
