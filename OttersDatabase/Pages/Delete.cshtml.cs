using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OttersDatabase.Models;
using OttersDatabase.Service;
using System.Threading.Tasks;

namespace OttersDatabase.Pages
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly OttersService _otterservice;

        public DeleteModel(OttersService service)
        {
            _otterservice = service;
        }
        public string GetUserId()
        {
            return HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? default;
        }

        [BindProperty]
        public Otter Otter { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Otter = await _otterservice.GetFullOtterAsync(id);
            if (Otter == null)
            {
                return NotFound();
            }
            if (Otter.founderID == GetUserId() || User.IsInRole("Administrator"))
            {
                return Page();
            }
            return LocalRedirect("~/Identity/Account/AccessDenied");
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (Otter != null)
            {
                await _otterservice.DeleteOtterAsync(id, GetUserId());
            }
            return RedirectToPage("./Index");
        }
    }
}