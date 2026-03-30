using DAL;
using BLL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<LibraryContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ILibraryService, LibraryService>();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();
    context.Database.EnsureCreated();
    
    if (!context.Locations.Any())
    {
        var service = scope.ServiceProvider.GetRequiredService<ILibraryService>();
        service.AddLocation("Сервер Київ", "вул. Хрещатик, 1");
        service.AddContent("Основи ООП", "Книга", "PDF", 1);
        service.AddContent("SOLID Патерни", "Відео", "MP4", 1);
    }
}

app.Run();