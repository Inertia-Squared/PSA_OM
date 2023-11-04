using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PSA_OM.Data;
using PSA_OM.Models;

namespace PSA_OM.Pages.Apartments
{
    [Authorize(Roles = "managers")]
    public class CreateModel : PageModel
    {
        private readonly PSA_OM.Data.ApplicationDbContext _context;

        public CreateModel(PSA_OM.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Apartment Apartment { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Apartment == null || Apartment == null)
            {
                return Page();
            }

            _context.Apartment.Add(Apartment);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
