using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OttersDatabase.Models;
using OttersDatabase.Service;
using System.Collections.Generic;
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (Otter != null)
            {
                await _otterservice.DeleteOtterAsync(id);
            }
            return RedirectToPage("./Index");
        }
    }
}