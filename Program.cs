using Microsoft.EntityFrameworkCore;
using rec_be;
using rec_be.Data;
using rec_be.Interfaces.Factory;
using rec_be.Interfaces.Repository;
using rec_be.Interfaces.Services;
using rec_be.Repository;
using rec_be.Room_FactoryStrategy.Factory;
using rec_be.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ── Configurar CORS ANTES de cualquier cosa ──────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        var allowedOrigins = new List<string>
        {
            "http://localhost:4200",
            "http://localhost:4201",
            "https://localhost:4200",
            "http://localhost:5173",
        };

        // Lee el origen del frontend desde variable de entorno
        var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL");
        if (!string.IsNullOrEmpty(frontendUrl))
            allowedOrigins.Add(frontendUrl);

        policy.WithOrigins(allowedOrigins.ToArray())
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });

builder.Services.AddOpenApi();

// PostgreSQL
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") 
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

// Railway entrega DATABASE_URL en formato URI, Npgsql necesita formato Key=Value
if (connectionString != null && connectionString.StartsWith("postgresql://"))
{
    var uri = new Uri(connectionString);
    var userInfo = uri.UserInfo.Split(':');
    connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={Uri.UnescapeDataString(userInfo[1])};SSL Mode=Require;Trust Server Certificate=true";
}

builder.Services.AddDbContext<RACPostgreSQLDbContext>(options =>
    options.UseNpgsql(connectionString)
           .UseSnakeCaseNamingConvention());

// ── Repositories ───────────────────────────────────────────────────
builder.Services.AddScoped<IRoomRepository,         PostgreSQLRoomRepository>();
builder.Services.AddScoped<IBookingRepository,      PostgreSQLBookingRepository>();
builder.Services.AddScoped<IGuestRepository,        PostgreSQLGuestRepository>();
builder.Services.AddScoped<IConfigRepository,       PostgreSQLConfigRepository>();
builder.Services.AddScoped<ILateCheckOutRepository, PostgreSQLLateCheckOutRepository>();

// ── Factory ───────────────────────────────────────────────────────
builder.Services.AddScoped<IRoomStrategyFactory, RoomStrategyFactory>();

// ── Services ───────────────────────────────────────────────────────
builder.Services.AddScoped<IBookingService,      BookingService>();
builder.Services.AddScoped<IRoomService,         RoomService>();
builder.Services.AddScoped<IGuestService,        GuestService>();
builder.Services.AddScoped<ILateCheckOutService, LateCheckOutService>();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();

app.UseRouting();

app.UseCors("AllowAngular");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "Residencial Al Cubo Web API";
    });
}

// ── Comentar temporalmente para desarrollo ──────────────────────
// app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();