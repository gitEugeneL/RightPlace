using API.Data;
using API.Middleware;
using API.Repositories;
using API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ErrorHandingMiddleware>();

builder.Services.AddControllers();

// Add DB connection -----------------------------------------------------------------------
builder.Services.AddDbContext<DataContext>(option =>
{
    // option.UseNpgsql(builder.Configuration.GetConnectionString("PgSQLConnection"));
    option.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection"));
});


// Add CORS --------------------------------------------------------------------------------
builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();