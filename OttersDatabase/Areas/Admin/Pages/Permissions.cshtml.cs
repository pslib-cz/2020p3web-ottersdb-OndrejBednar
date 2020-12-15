using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OttersDatabase.Models;
using OttersDatabase.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OttersDatabase.Areas.Admin.Pages
{
    [Authorize(Policy = "Admin")]
    public class PermissionsModel : PageModel
    {
        private readonly OtterDbContext _context;
        public PermissionsModel(OtterDbContext context)
        {
            _context = context;
        }

        public string GetUserId()
        {
            return HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? default;
        }

        public class Permissions
        {
            public string User { get; set; }
            public string Role { get; set; }
        }
        public List<SelectListItem> Users { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Roles { get; set; } = new List<SelectListItem>();
        [BindProperty]
        public Permissions Set { get; set; } = new Permissions();


        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnGetAsync()
        {
            
            await foreach (var item in _context.Users.AsAsyncEnumerable())
            {
                if (User.Identity.Name == item.UserName)
                {
                    continue;
                }
                else
                {
                Users.Add(new SelectListItem($"{item.UserName}", $"{item.Id}"));
                }
            }
            await foreach (var item in _context.Roles.AsAsyncEnumerable())
            {
                Roles.Add(new SelectListItem($"{item.Name}", $"{item.Id}"));
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            /*if (!ModelState.IsValid)
            {
                return Page();
            }*/
            if (Set.Role == null)
            {
                return RedirectToPage("./Index");
            }
            if (_context.UserRoles.Find( new string[] { Set.User, Set.Role}) == null)
            {
                _context.UserRoles.Add(new IdentityUserRole<string> { UserId = Set.User, RoleId = Set.Role });
            }
 
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
