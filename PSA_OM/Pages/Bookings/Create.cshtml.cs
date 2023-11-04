using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PSA_OM.Data;
using PSA_OM.Models;

namespace PSA_OM.Pages.Bookings
{
    public class CreateModel : PageModel
    {
        private readonly PSA_OM.Data.ApplicationDbContext _context;

        public CreateModel(PSA_OM.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ApartmentID"] = new SelectList(_context.Apartment, "ID", "Level");
        ViewData["TravellerEmail"] = new SelectList(_context.Traveller, "Email", "Email");
            return Page();
        }

        [BindProperty]
        public Booking Booking { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Booking == null || Booking == null)
            {
                return Page();
            }

            _context.Booking.Add(Booking);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
