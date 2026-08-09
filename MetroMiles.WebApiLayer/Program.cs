using System.Threading.RateLimiting;
using Core.CrossCuttingConcernLayer.ExceptionHandlings.Extensions;
using Core.SecurityLayer.Encryptions;
using Core.SecurityLayer.Extensions;
using Core.SecurityLayer.JsonWebTokens.Concretions;
using MetroMiles.ApplicationLayer.Extensions.ServiceRegistrations;
using MetroMiles.PersistenceLayer.Context;
using MetroMiles.PersistenceLayer.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

const string DevelopmentCorsPolicy = "DevelopmentCorsPolicy";
const string AuthRateLimiterPolicy = "AuthRateLimiterPolicy";
const string TokenSecurityKeyMissingMessage =
    "'TokenOptions:SecurityKey' is not configured. Set it via user-secrets " +
    "(dotnet user-secrets set 'TokenOptions:SecurityKey' '<value>') in development, " +
    "or an environment variable/secret store in other environments — never in appsettings.json.";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddPersistanceServices(builder.Configuration);
builder.Services.AddSecurityServices();
builder.Services.AddHttpContextAccessor();

builder.Services.AddOptions<TokenOption>()
    .BindConfiguration("TokenOptions")
    .Validate(tokenOptions => !string.IsNullOrWhiteSpace(tokenOptions.SecurityKey), TokenSecurityKeyMissingMessage)
    .ValidateOnStart();
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<TokenOption>>().Value);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

builder.Services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
    .Configure<IOptions<TokenOption>>((jwtBearerOptions, tokenOptionsAccessor) =>
    {
        var tokenOptions = tokenOptionsAccessor.Value;
        jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddAuthorization();

var corsAllowedOrigins = builder.Configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>()
    ?? ["http://localhost:4200"]; // Angular dev server default; override via CorsSettings:AllowedOrigins.

builder.Services.AddCors(options =>
{
    // Development-only policy, scoped to the known frontend origin(s).
    options.AddPolicy(DevelopmentCorsPolicy, policy => policy.WithOrigins(corsAllowedOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    // Scoped to credential-guessing surfaces (login/refresh) only — partitioned per client IP so one
    // abusive caller can't exhaust the limit for everyone else.
    options.AddPolicy(AuthRateLimiterPolicy, httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
            }));
});

// Falls back to in-process caching when no Redis connection is configured, so the app still runs
// (e.g. local dev without Redis installed) instead of failing on every cache-behavior request.
var redisConnection = builder.Configuration.GetConnectionString("Redis");
if (string.IsNullOrWhiteSpace(redisConnection))
{
    builder.Services.AddDistributedMemoryCache();
}
else
{
    builder.Services.AddStackExchangeRedisCache(opt => opt.Configuration = redisConnection);
}

var databaseConnection = builder.Configuration.GetRequiredConnectionString("FakeDatabaseName");

var healthChecksBuilder = builder.Services.AddHealthChecks()
    .AddSqlServer(databaseConnection, name: "sql-server");
if (!string.IsNullOrWhiteSpace(redisConnection))
{
    healthChecksBuilder.AddRedis(redisConnection, name: "redis");
}


builder.Services.AddControllers();
builder.Services.AddConfigureCustomModelValidation();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
        };
        return Task.CompletedTask;
    });
    options.AddOperationTransformer((operation, context, cancellationToken) =>
    {
        var metadata = context.Description.ActionDescriptor.EndpointMetadata;
        var requiresAuth = metadata.OfType<AuthorizeAttribute>().Any() && !metadata.OfType<IAllowAnonymous>().Any();
        if (requiresAuth)
        {
            operation.Security ??= [];
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", context.Document)] = []
            });
        }
        return Task.CompletedTask;
    });
});


var app = builder.Build();
using (var migrationScope = app.Services.CreateScope())
{
    migrationScope.ServiceProvider.GetRequiredService<BaseDbContext>().Database.Migrate();
    await IdentitySeeder.SeedIdentityDataAsync(migrationScope.ServiceProvider);
}

app.UseConfigureCustomExceptionMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(DevelopmentCorsPolicy);
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
