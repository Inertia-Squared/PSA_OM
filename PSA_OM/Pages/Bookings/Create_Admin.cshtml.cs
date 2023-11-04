using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PSA_OM.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PSA_OM.Pages.Bookings
{
    [Authorize(Roles = "managers")]
    public class Create_AdminModel : PageModel
    {
        private readonly PSA_OM.Data.ApplicationDbContext _context;

        public Create_AdminModel(PSA_OM.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["ApartmentID"] = new SelectList(_context.Apartment, "ID", "ID");
            ViewData["TravellerEmail"] = new SelectList(await _context.Traveller
                                                             .Select(t => new { t.Email, t.FullName })
                                                             .ToListAsync(),
                                                             "Email", "FullName");
            return Page();
        }

        [BindProperty]
        public Booking Booking { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["TravellerEmail"] = new SelectList(await _context.Traveller
                                                                 .Select(t => new { t.Email, t.FullName })
                                                                 .ToListAsync(),
                                                                 "Email", "FullName");
                return Page();
            }

            string sql = @"
            SELECT COUNT(*) FROM Booking 
            WHERE ApartmentID = @p0 
            AND NOT (
                CheckOut <= @p1 OR 
                CheckIn >= @p2
            )";

            var overlappingBookings = await _context.Booking
                .FromSqlRaw(sql, Booking.ApartmentID, Booking.CheckIn, Booking.CheckOut)
                .CountAsync();

            if (overlappingBookings > 0)
            {
                ModelState.AddModelError("", "The booking is unavailable for the selected dates.");
                return Page();
            }

            _context.Booking.Add(Booking);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
