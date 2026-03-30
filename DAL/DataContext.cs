using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class StorageLocation { public int Id { get; set; } public string Name { get; set; } public string PhysicalAddress { get; set; } public List<ContentItem> Contents { get; set; } = new(); }
    public class ContentItem { public int Id { get; set; } public string Title { get; set; } public string ContentType { get; set; } public string Format { get; set; } public int StorageLocationId { get; set; } public StorageLocation StorageLocation { get; set; } }

    public class LibraryContext : DbContext
    {
        public DbSet<ContentItem> Contents { get; set; }
        public DbSet<StorageLocation> Locations { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { optionsBuilder.UseSqlite("Data Source=library.db"); }
    }

    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Delete(T entity);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly LibraryContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(LibraryContext context) { _context = context; _dbSet = context.Set<T>(); }
        public IEnumerable<T> GetAll() => _dbSet.ToList();
        public T GetById(int id) => _dbSet.Find(id);
        public void Add(T entity) => _dbSet.Add(entity);
        public void Delete(T entity) => _dbSet.Remove(entity);
    }

    public interface IUnitOfWork { IGenericRepository<ContentItem> Contents { get; } IGenericRepository<StorageLocation> Locations { get; } void Save(); }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryContext _context;
        private IGenericRepository<ContentItem> _contents;
        private IGenericRepository<StorageLocation> _locations;
        public UnitOfWork(LibraryContext context) { _context = context; }
        public IGenericRepository<ContentItem> Contents => _contents ??= new GenericRepository<ContentItem>(_context);
        public IGenericRepository<StorageLocation> Locations => _locations ??= new GenericRepository<StorageLocation>(_context);
        public void Save() => _context.SaveChanges();
    }
}