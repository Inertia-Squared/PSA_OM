using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PSA_OM.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PSA_OM.Pages.Bookings
{
    [Authorize(Roles = "managers")]
    public class EditModel : PageModel
    {
        private readonly PSA_OM.Data.ApplicationDbContext _context;

        public EditModel(PSA_OM.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Booking Booking { get; set; }



        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Booking = await _context.Booking.FindAsync(id);

            if (Booking == null)
            {
                return NotFound();
            }

            ViewData["ApartmentID"] = new SelectList(_context.Apartment, "ID", "ID");
            ViewData["TravellerEmail"] = new SelectList(await _context.Traveller
                                                             .Select(t => new { t.Email, t.FullName })
                                                             .ToListAsync(),
                                                             "Email", "FullName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // this one query took like 5 years off my life. For some reason the validation just refused to work without a low-level query dfsdkjfhsdfjkhsd
            string sql = @"
            SELECT COUNT(*) FROM Booking
            WHERE ApartmentID = @ApartmentID
            AND ID <> @CurrentBookingID
            AND (
                (CheckIn < @CheckOutDate AND CheckOut > @CheckInDate)
            )";

            var apartmentIdParam = new SqliteParameter("@ApartmentID", Booking.ApartmentID);
            var currentBookingIdParam = new SqliteParameter("@CurrentBookingID", Booking.ID);
            var checkInDateParam = new SqliteParameter("@CheckInDate", Booking.CheckIn);
            var checkOutDateParam = new SqliteParameter("@CheckOutDate", Booking.CheckOut);

            int countOfOverlappingBookings = 0;

            var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(apartmentIdParam);
                command.Parameters.Add(currentBookingIdParam);
                command.Parameters.Add(checkInDateParam);
                command.Parameters.Add(checkOutDateParam);

                var result = await command.ExecuteScalarAsync();
                countOfOverlappingBookings = Convert.ToInt32(result);
            }

            await connection.CloseAsync();



            if (countOfOverlappingBookings > 0)
            {
                ModelState.AddModelError("", "The booking is unavailable for the selected dates.");
                if (Request.Headers.ContainsKey("Referer"))
                {
                    var referer = Request.Headers["Referer"].ToString();
                    return Redirect(referer);
                }
                return Page();
            }

            _context.Attach(Booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(Booking.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            if (Request.Headers.ContainsKey("Referer"))
            {
                var referer = Request.Headers["Referer"].ToString();
                return Redirect(referer);
            }
            return Page();
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.ID == id);
        }
    }
}
