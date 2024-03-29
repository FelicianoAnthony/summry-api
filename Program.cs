using LbAutomationPortalApi.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using SummryApi.Constants;
using SummryApi.Helpers.AuthHelper;
using SummryApi.Middlewares.Authorizations;
using SummryApi.Middlewares.Exceptions;
using SummryApi.Repositories.UnitOfWork;
using SummryApi.Services.HttpClients;
using SummryApi.Services.Permissions;
using SummryApi.Services.Platforms;
using SummryApi.Services.Products;
using SummryApi.Services.RolePermissions;
using SummryApi.Services.Roles;
using SummryApi.Services.Stores;
using SummryApi.Services.Users;
using SummryApi.Services.UserSummries;
using SummryApi.Services.UserSummryQueryService;
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
builder.Services.AddDbContext<SummryContext>(options =>
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
builder.Services.AddScoped<IAuthHelpers, AuthHelpers>();
builder.Services.AddScoped<IPlatformService, PlatformService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();
builder.Services.AddScoped<IUserSummryService, UserSummryService>();
builder.Services.AddScoped<IUserSummryQueryService, UserSummryQueryService>();


// add class that maps to 'JwtConfig' key in appsettings.{{environment}}.json 
builder.Services.AddSingleton(builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>());
builder.Services.AddSingleton(builder.Configuration.GetSection("RegexConfig").Get<RegexConfig>());
builder.Services.AddSingleton(builder.Configuration.GetSection("ScraperPlatformConfig").Get<ScraperPlatformConfig>());


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// add HTTP classes
builder.Services.AddScoped<ScrapeApprovalClient>();

// prevents this error, only started when using automapper - https://stackoverflow.com/questions/59199593/net-core-3-0-possible-object-cycle-was-detected-which-is-not-supported
// this line also makes this type of null removal not work in ApiModels class properties - [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
// [JsonProperty] data annotation in ./ApiModels started acting differently than before after this line too...
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);


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
