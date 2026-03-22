using DAL;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class ContentDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ContentType { get; set; }
        public string Format { get; set; }
    }

    public class LocationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public interface ILibraryService
    {
        void AddLocation(string name, string address);
        void AddContent(string title, string type, string format, int locationId);
        IEnumerable<ContentDto> SearchContent(string keyword);
        IEnumerable<LocationDto> GetAllLocations();
    }

    public class LibraryService : ILibraryService
    {
        private readonly IUnitOfWork _uow;

        public LibraryService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void AddLocation(string name, string address)
        {
            _uow.Locations.Add(new StorageLocation { Name = name, PhysicalAddress = address });
            _uow.Save();
        }

        public void AddContent(string title, string type, string format, int locationId)
        {
            _uow.Contents.Add(new ContentItem 
            { 
                Title = title, ContentType = type, Format = format, StorageLocationId = locationId 
            });
            _uow.Save();
        }

        public IEnumerable<LocationDto> GetAllLocations()
        {
            return _uow.Locations.GetAll().Select(l => new LocationDto { Id = l.Id, Name = l.Name }).ToList();
        }

        public IEnumerable<ContentDto> SearchContent(string keyword)
        {
            var results = _uow.Contents.GetAll()
                .Where(c => c.Title.ToLower().Contains(keyword.ToLower()));

            return results.Select(c => new ContentDto 
            { 
                Id = c.Id, Title = c.Title, ContentType = c.ContentType, Format = c.Format 
            }).ToList();
        }
    }
}