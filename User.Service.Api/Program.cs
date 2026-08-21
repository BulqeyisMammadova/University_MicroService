using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using User.Servic.Business.Options;
using User.Servic.Business.Services.Abstractions;
using User.Servic.Business.Services.Implementations;
using User.Service.Api.Middlewares;
using User.Service.Business.Clients.Implementations;
using User.Service.Business.Services.Abstractions;
using User.Service.Business.Services.Implementations;
using User.Service.DataAccess.Data;
using User.Service.DataAccess.Repositories.Abstarctions;
using User.Service.DataAccess.Repositories.Implementations;

var builder = WebApplication.CreateBuilder(args);

var logPath = builder.Configuration["LogSettings:Path"];
GlobalDiagnosticsContext.Set("logDirectory", logPath!);

builder.Host.UseNLog();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<MailOptions>(builder.Configuration.GetSection("MailOptions"));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddHttpClient<IAuthServiceClient, AuthServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AuthServiceUrl"]!);
});

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