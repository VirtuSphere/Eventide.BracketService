using Microsoft.EntityFrameworkCore;
using BracketService.Data;
using BracketService.Services;

var builder = WebApplication.CreateBuilder(args);

// Добавляем gRPC
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

// Добавляем контекст базы данных
builder.Services.AddDbContext<BracketDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавляем генератор
builder.Services.AddSingleton<IBracketGenerator, BracketGenerator>();

var app = builder.Build();

// Применяем миграции при запуске
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BracketDbContext>();
    dbContext.Database.Migrate();
}

// Настраиваем маршруты
app.MapGrpcService<BracketGrpcService>();
// Включаем рефлексию для разработки
if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}
app.MapGet("/", () => "Bracket Service is running...");

app.Run();