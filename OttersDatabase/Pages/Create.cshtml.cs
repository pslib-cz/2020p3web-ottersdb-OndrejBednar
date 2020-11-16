using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OttersDatabase.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OttersDatabase.Service;

namespace OttersDatabase.Pages
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly OttersService _otterservice;
        public CreateModel(OttersService service)
        {
            _otterservice = service;
        }

        public string GetUserId()
        {
            return HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? default;
        }


        public List<SelectListItem> PlaceNames { get; set; }

        public List<SelectListItem> MotherIds { get; set; }

        [BindProperty]
        public Otter Otter { get; set; }


        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnGet()
        {
            _otterservice.PrepareSelectLists();
            PlaceNames = _otterservice.PlaceNames;
            MotherIds = _otterservice.MotherIds;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            /*if (!ModelState.IsValid)
            {
                return Page();
            }*/

            Otter = await _otterservice.CreateOtterAsync(Otter, GetUserId());
            return RedirectToPage("./Index");
        }
    }
}
