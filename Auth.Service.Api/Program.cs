using Auth.Service.Business.Services.Abstarctions;
using Auth.Service.Business.Services.Abstractions;
using Auth.Service.Business.Services.Implementations;
using Auth.Service.DataAccess.Data;
using Auth.Service.DataAccess.Repositories.Abstarctions;
using Auth.Service.DataAccess.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Auth.Service.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var logPath = builder.Configuration["LogSettings:Path"];
GlobalDiagnosticsContext.Set("logDirectory", logPath!);
builder.Host.UseNLog();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IRefreshTokenStore, RefreshTokenStore>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();