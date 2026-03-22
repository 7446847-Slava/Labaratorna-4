using System;
using System.Linq;
using DAL;
using BLL;

namespace UI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Бібліотека Контенту (N-Tier Architecture) ===");

            using var context = new LibraryContext();
            context.Database.EnsureCreated(); 

            IUnitOfWork uow = new UnitOfWork(context);
            ILibraryService service = new LibraryService(uow);

            Console.WriteLine("\nІніціалізація сховищ...");
            service.AddLocation("Сервер Київ", "вул. Хрещатик, 1");
            service.AddLocation("Хмарне сховище AWS", "eu-central-1");

            var locations = service.GetAllLocations();
            // Виправлений і безпечний спосіб отримати ID:
            int firstLocId = locations.FirstOrDefault()?.Id ?? 1;

            Console.WriteLine("Додавання контенту...");
            service.AddContent("Основи ООП", "Книга", "PDF", firstLocId);
            service.AddContent("SOLID Патерни", "Відео", "MP4", firstLocId);
            service.AddContent("Лекція з Архітектури", "Аудіо", "MP3", firstLocId);

            Console.WriteLine("\n--- Пошук контенту: 'ООП' ---");
            var searchResults = service.SearchContent("ООП");
            
            foreach (var item in searchResults)
            {
                Console.WriteLine($"Знайдено: [{item.ContentType}] {item.Title} (Формат: {item.Format})");
            }

            Console.WriteLine("\n--- Пошук контенту: 'Архітектура' ---");
            var searchResults2 = service.SearchContent("Архітектура");
            foreach (var item in searchResults2)
            {
                Console.WriteLine($"Знайдено: [{item.ContentType}] {item.Title} (Формат: {item.Format})");
            }

            Console.WriteLine("\nРоботу завершено. Натисніть Enter...");
            Console.ReadLine();
        }
    }
}