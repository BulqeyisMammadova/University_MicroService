using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using NLog;
using NLog.Web;
using Student.Service.Api.Middlewares;
using Student.Service.Business.Services.Abstarctions;
using Student.Service.Business.Services.Implementations;
using Student.Service.DataAccess.Data;
using Student.Service.DataAccess.Repositories.Abstarctions;
using Student.Service.DataAccess.Repositories.Implementations;

var builder = WebApplication.CreateBuilder(args);

var logPath = builder.Configuration["LogSettings:Path"];
GlobalDiagnosticsContext.Set("logDirectory", logPath!);
builder.Host.UseNLog();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Token-i bura yaz (Bearer sözü olmadan)."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        { new OpenApiSecuritySchemeReference("Bearer", document), new List<string>() }
    });
});

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