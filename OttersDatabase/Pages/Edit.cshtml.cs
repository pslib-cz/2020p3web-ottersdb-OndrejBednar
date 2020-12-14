using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OttersDatabase.Models;
using OttersDatabase.Service;

namespace OttersDatabase.Pages
{
    public class EditModel : PageModel
    {
        private readonly OttersService _otterservice;

        public EditModel(OttersService service)
        {
            _otterservice = service;
        }
        public string GetUserId()
        {
            return HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? default;
        }


        [BindProperty]
        public Otter Otter { get; set; }
        [BindProperty]
        public string Data { get; set; }
        public List<SelectListItem> MotherIds { get; set; }
        public List<SelectListItem> PlaceNames { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
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
            _otterservice.PrepareSelectLists();
            MotherIds = _otterservice.MotherIds;
            PlaceNames = _otterservice.PlaceNames;
            Otter.PlaceName = null;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            /*if (!ModelState.IsValid)
            {
                return Page();
            }*/
            if (await _otterservice.EditOtterAsync(Otter, GetUserId()) == false) { return NotFound(); }

            return RedirectToPage("./Index");
        }


    }
}