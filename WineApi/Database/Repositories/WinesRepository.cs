using Microsoft.EntityFrameworkCore;
using WineApi.Database.Entities;

namespace WineApi.Database.Repositories
{
    public class WinesRepository(ApplicationDbContext context) : IRepository<Wine>
    {
        private readonly ApplicationDbContext _context = context;
        private bool _disposedValue;

        public void Add(Wine entity)
        {
            _context.Wines.Add(entity);
        }

        public void Delete(int id)
        {
            var wine = _context.Wines.Find(id);
            if (wine != null)
                _context.Wines.Remove(wine);
        }

        public IEnumerable<Wine> GetAll()
        {
            return _context.Wines;
        }

        public Wine? GetById(int id)
        {
            return _context.Wines.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Wine wine)
        {
            var existingWine = _context.Wines.FirstOrDefault(w => w.Id == wine.Id);
            if (existingWine != null)
            {
                existingWine.Title = wine.Title;
                existingWine.Type = wine.Type;
                existingWine.Year = wine.Year;
                existingWine.Brand = wine.Brand;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
