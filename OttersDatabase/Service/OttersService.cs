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
            foreach (var item in _context.Places.Include(l => l.Location).AsEnumerable())
            {
                if (Otter != null && item.Name == Otter.Place.Name && item.LocationId == Otter.Place.LocationId)
                {
                    PlaceNames.Add(new SelectListItem($"{item.Name} ({item.Location.Name})",
                                               $"{item.Name};{item.LocationId}", true));
                }
                else
                {
                    PlaceNames.Add(new SelectListItem($"{item.Name} ({item.Location.Name})",
                                               $"{item.Name};{item.LocationId}"));
                }
            }
        }
        public async Task<Otter> CreateOtterAsync(Otter otter, string UserID)
        {
            string[] data;
            data = otter.PlaceName.Split(';');
            Otter = new Otter()
            {
                Name = otter.Name,
                Color = otter.Color,
                MotherId = otter.MotherId,
                PlaceName = data[0],
                LocationId = int.Parse(data[1]),
                founderID = UserID
            };

            _context.Otters.Add(Otter);
            await _context.SaveChangesAsync();
            return Otter;
        }
        public async Task<bool> DeleteOtterAsync(int? id, string userId)
        {
            Otter = await GetFullOtterAsync(id);
            if (Otter.Children != null)
            {
                foreach (var item in Otter.Children)
                {
                    _context.Otters.Find(item.TattooID).MotherId = null;
                }
            }
            if (Otter.founderID == userId || _context.UserRoles.Find(new [] { userId, "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXX1" }) != null)
            {
                _context.Otters.Remove(await _context.Otters.FindAsync(id));
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> EditOtterAsync(Otter otter, string UserID)
        {
            string[] data;
            data = otter.PlaceName.Split(';');
            Otter = new Otter()
            {
                TattooID = otter.TattooID,
                Name = otter.Name,
                Color = otter.Color,
                MotherId = otter.MotherId,
                PlaceName = data[0],
                LocationId = int.Parse(data[1]),
                founderID = otter.founderID
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
        public async Task<Otter> GetOtterAsync(int? id)
        {
            Otter = await _context.Otters.AsNoTracking().FirstOrDefaultAsync(m => m.TattooID == id);
            return Otter;
        }
        public async Task<Otter> GetFullOtterAsync(int? id)
        {
            Otter = await _context.Otters
                    .Include(v => v.Location)
                    .Include(v => v.Mother)
                    .Include(v => v.Place)
                    .Include(v => v.Children)
                    .Include(v => v.founder).AsNoTracking().FirstOrDefaultAsync(m => m.TattooID == id);
            return Otter;
        }
    }
}
