using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OttersDatabase.Models;
using OttersDatabase.Service;

namespace OttersDatabase.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly OttersService _otterservice;

        public DetailsModel(OttersService service)
        {
            _otterservice = service;
        }

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
    }
}