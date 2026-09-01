using System.Text;
using ApiGateway.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var logPath = builder.Configuration["LogSettings:Path"];
GlobalDiagnosticsContext.Set("logDirectory", logPath!);
builder.Host.UseNLog();
var ocelotFileName = builder.Environment.EnvironmentName == "Docker"
    ? "ocelot.docker.json"
    : "ocelot.json";
builder.Configuration.AddJsonFile(ocelotFileName, optional: false, reloadOnChange: true);

var jwtSecretKey = builder.Configuration["JWTOptions:SecretKey"]!;
var jwtIssuer = builder.Configuration["JWTOptions:Issuer"]!;
var jwtAudience = builder.Configuration["JWTOptions:Audience"]!;


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero 
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseAuthentication();
await app.UseOcelot();
app.UseAuthentication();
await app.UseOcelot();

app.Run();