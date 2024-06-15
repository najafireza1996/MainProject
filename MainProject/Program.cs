using Features.Core.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ISession = Common.Managers.SessionManager.ISession;
using Common.Helpers;
using Common.Managers.SessionManager;
using Features.Authentication.Application.IServices;
using Features.Authentication.Application.Services;
using Features.Core.Application.IServices;
using Features.Core.Application.Services;
using Features.Core.Domain.IRepositories;
using Features.User.Application.IServices;
using Features.User.Application.Services;
using Features.User.Domain.IRepositories;
using Features.User.Infrastructure.Repositories;
using Infrastructure.Repositories;
using Serilog;
using Domain.Filters;
using Common.Constants;
using Features.User.Domain.Entities;
using System.Text;
using Features.Authentication.Domain.Filters;
using System.Reflection;
using Infrastructure.DataAccess;
using Features.BackOffice.Application.Services;
using Features.BackOffice.Application.IServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .EnableSensitiveDataLogging(true));



// Add services to the container.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ISession, Session>();


builder.Services.AddControllers();
builder.Services.AddScoped<SuspenededActionFilter>();
builder.Services.AddRouting(options => options.LowercaseUrls = true);


// logging 
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


// Identity configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.SignIn.RequireConfirmedEmail = false;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.User.AllowedUserNameCharacters = UserConstants.AllowedUserNameCharacters;
}).AddEntityFrameworkStores<AppDbContext>();


var key = Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT")["KEY"]);
var tokenValidationParams = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = false,
    RequireExpirationTime = false,
    ClockSkew = TimeSpan.Zero,
};

builder.Services.AddSingleton(tokenValidationParams);
// Authentication configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt =>
{
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = tokenValidationParams;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// add swaggerGen 
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MainProject", Version = "v1" });
    c.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme.ToLowerInvariant(),
        In = ParameterLocation.Header,
        Name = "Authorization",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.DescribeAllParametersInCamelCase();
    c.OrderActionsBy(x => x.RelativePath);
    c.OperationFilter<AuthResponsesOperationFilter>();

    var xmlfile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlfile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();

string imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"Resources/Images");
if (!Directory.Exists(imagesDirectory))
{
    Directory.CreateDirectory(imagesDirectory);
}

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
                        Path.Combine(Directory.GetCurrentDirectory(), @"Resources/Images")),
    RequestPath = new PathString("/app-images")
});

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

await app.SeedDataAsync();
app.Run();

