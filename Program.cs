using LbAutomationPortalApi.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using StarterApi.Constants;
using StarterApi.Helpers;
using StarterApi.Middlewares.Authorizations;
using StarterApi.Middlewares.Exceptions;
using StarterApi.Repositories.UnitOfWork;
using StarterApi.Services.HttpClients;
using StarterApi.Services.Permissions;
using StarterApi.Services.Platforms;
using StarterApi.Services.Products;
using StarterApi.Services.Queries;
using StarterApi.Services.RolePermissions;
using StarterApi.Services.Roles;
using StarterApi.Services.Stores;
using StarterApi.Services.Users;
using StarterApi.Services.UserStores;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
builder.Logging.ClearProviders();
builder.Host.UseNLog();


// add logic to start API with different appsettings.{{environment}}.json file
IWebHostEnvironment environment = builder.Environment;
builder.Configuration
    .SetBasePath(environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    //.AddUserSecrets<Program>() // secrets.json file
    .Build();

logger.Info($"initializing project with '{environment.EnvironmentName}' appsettings file");




// Add services to the container.

// get DB connection string & use default DB schema 
builder.Services.AddDbContext<StarterApiContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
         x => x.MigrationsHistoryTable("__EFMigrationsHistory", builder.Configuration.GetValue<string>("DefaultSchema"))
));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// add 'Authorize' button on swagger page -- add it with 'Bearer' in front of token...
builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                       {
                         new OpenApiSecurityScheme
                         {
                           Reference = new OpenApiReference
                           {
                             Type = ReferenceType.SecurityScheme,
                             Id = "Bearer"
                           }
                          },
                          new string[] { }
                        }
                });
            });


// add JWT validation
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JwtConfig:JwtSecret"))),
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = false,
        ValidateLifetime = false
    };

    // look for request cookie called token - {Cookie: token=<jwt>} & disable default Authorization JWT header
    options.Events = new JwtBearerEvents
    {
        // https://stackoverflow.com/questions/41955117/custom-token-location-for-jwtbearermiddleware
        OnMessageReceived = context =>
        {
            string jwtCookieName = builder.Configuration.GetValue<string>("JwtConfig:JwtCookieName");
            string defaultAuthHeader = builder.Configuration.GetValue<string>("JwtConfig:DefaultAuthHeader");

            // THIS IS WEIRD: null out default Bearer authorization header (if passed, but allow if request from swagger page) & handle it in cookie instead...
            var isRequestFromSwagger = context.Request.Headers.Where(h => h.Key == "Referer").FirstOrDefault().Value.ToString().Contains("swagger");
            var authHeader = context.Request.Headers.Where(h => h.Key == defaultAuthHeader).FirstOrDefault(); // purposely outside if statement...
            if (!isRequestFromSwagger)
            {
                // if request isn't from swagger, remove header, if header even present
                if (authHeader.Key != null) { context.Request.Headers[authHeader.Key] = ""; }
            }

            context.Token = context.Request.Cookies[jwtCookieName];

            return Task.CompletedTask;
        }
    };
});


// add authorization roles 
builder.Services.AddAuthorization(options =>
{
    var jwtUsername = builder.Configuration.GetValue<string>("JwtConfig:jwtUsername");

    options.AddPolicy("USERNAME_POLICY", policy =>
    {
        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(new UserNameRequirement(jwtUsername));
    });

    options.AddPolicy("USERNAME_AND_ROLE_POLICY", policy => 
    {
        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
        policy.RequireAssertion(ctx =>
            ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value == jwtUsername 
            // && ctx.User.HasClaim(c => c.Value == "SOME_ROLE")
        );
    });
});

// add authorization roles - required for policy.AddRequirements to work...
builder.Services.AddSingleton<IAuthorizationHandler, UsernameAuthorization>();

// add developer-defined Services 
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IHelper, Helper>();
builder.Services.AddScoped<IPlatformService, PlatformService>();
builder.Services.AddScoped<IQueryService, QueryService>();
builder.Services.AddScoped<IUserStoreService, UserStoreService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();


// add class that maps to 'JwtConfig' key in appsettings.{{environment}}.json 
builder.Services.AddSingleton(builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>());
builder.Services.AddSingleton(builder.Configuration.GetSection("RegexConfig").Get<RegexConfig>());


// add HTTP client service config 
builder.Services.AddHttpClient<HttpClientService>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("HttpClientConfigs:ClientConfig:url"));
});

// serializer services -- // this hides null values globally 
//builder.Services.AddMvc()
//        .AddJsonOptions(options => {
//            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
//        });



var app = builder.Build();

// add custom middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseMiddleware<NLog.Web.NLogRequestPostedBodyMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection(); // COMMENTED OUT BECAUSE SERVER DOESNT USE HTTPS YET...

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // MUST be before app.UseAuthentication...
app.UseMiddleware<AuthenticationErrorMiddleware>();

app.UseAuthentication()
    .UseAuthorization();

app.MapControllers();

app.Run();
