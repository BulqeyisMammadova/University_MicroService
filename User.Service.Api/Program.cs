using Microsoft.EntityFrameworkCore;
using User.Servic.Business.Services.Abstractions;
using User.Service.Business.Clients.Implementations;
using User.Service.Business.Services.Abstractions;
using User.Service.Business.Services.Implementations;
using User.Service.DataAccess.Data;
using User.Service.DataAccess.Repositories.Abstarctions;
using User.Service.DataAccess.Repositories.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();          
builder.Services.AddScoped<IPermissionService, PermissionService>(); 


//Zeng
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

// ✅ Yeni əlavə: CORS
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//: CORS middleware
app.UseCors("AllowAll");


app.UseHttpsRedirection();
app.MapControllers();

app.Run();
