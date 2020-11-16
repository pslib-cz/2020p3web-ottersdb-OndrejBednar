using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OttersDatabase.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OttersDatabase.Service
{
    public class OttersService
    {
        private readonly OtterDbContext _context;

        public OttersService(OtterDbContext context)
        {
            _context = context;
        }

        private Otter Otter { get; set; }
        public List<SelectListItem> PlaceNames { get; set; }
        public List<SelectListItem> MotherIds { get; set; }


        public void PrepareSelectLists()
        {
            MotherIds = new List<SelectListItem>();
            foreach (var item in _context.Otters)
            {
                MotherIds
                    .Add(new SelectListItem($"{item.Name}", $"{item.TattooID}"));
            }

            PlaceNames = new List<SelectListItem>();
            foreach (var item in _context.Places.Include(l => l.Location).AsEnumerable<Place>())
            {
                PlaceNames
                    .Add(new SelectListItem($"{item.Name} ({item.Location.Name})",
                                               $"{item.LocationId};{item.Name}"));
            }
        }
        public async Task<Otter> CreateOtterAsync(Otter otter, string UserID)
        {
            string[] data;
            data = otter.PlaceName.Split(';');
            Otter = new Otter()
            {
                founderID = UserID,
                MotherId = otter.MotherId,
                Name = otter.Name,
                Color = otter.Color,
                LocationId = int.Parse(data[0]),
                PlaceName = data[1]
            };

            _context.Otters.Add(Otter);
            await _context.SaveChangesAsync();
            return Otter;
        }
        public async Task<Otter> GetOtterAsync(int? id)
        {
            Otter = await _context.Otters.FindAsync(id);
            return Otter;
        }
        public async Task<Otter> GetFullOtterAsync(int? id)
        {
            Otter = await _context.Otters
                    .Include(v => v.Location)
                    .Include(v => v.Mother)
                    .Include(v => v.Place)
                    .Include(v => v.founder).AsNoTracking().FirstOrDefaultAsync(m => m.TattooID == id);
            return Otter;
        }
        public async Task<string> DeleteOtterAsync(int? id)
        {
            _context.Otters.Remove(await _context.Otters.FindAsync(id));
            await _context.SaveChangesAsync();
            return "done";
        }
        public async Task<bool> EditOtterAsync(Otter otter, string UserID)
        {
            string[] data;
            data = otter.PlaceName.Split(';');
            Otter = new Otter()
            {
                TattooID = otter.TattooID,
                founderID = UserID,
                Mother = otter.Mother,
                MotherId = otter.MotherId,
                Name = otter.Name,
                Color = otter.Color,
                LocationId = int.Parse(data[0]),
                PlaceName = data[1]
            };
            _context.Attach(Otter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
    }
}
