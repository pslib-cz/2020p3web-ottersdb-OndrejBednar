using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OttersDatabase.Models;

namespace OttersDatabase.Pages
{
    public class IndexModel : PageModel
    {
        private readonly OtterDbContext _context;

        public IndexModel(OtterDbContext context)
        {
            _context = context;
        }

        public string GetUserId()
        {
            return HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? default;
        }

        public IEnumerable<Otter> Otters { get; set; }

        public void OnGet()
        {
            Otters = _context.Otters
                .Include(v => v.Location)
                .Include(v => v.Mother)
                .Include(v => v.Place)
                .Include(v => v.founder);
        }
    }
}
