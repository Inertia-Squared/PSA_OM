using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PSA_OM.CustomAttributes;
using PSA_OM.Data;
using PSA_OM.Models;

namespace PSA_OM.Pages
{
    public class BookModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public BookModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BookingViewModel Booking { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsBookingAvailable { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool BookingInserted { get; set; }

        public string ApartmentLevel { get; set; }
        public decimal BookingCost { get; set; }

        public IActionResult OnGet()
        {
            IsBookingAvailable = true;
            BookingInserted = false;
            string Email = User.FindFirst(ClaimTypes.Name).Value;
            Traveller traveller = _context.Traveller.FirstOrDefault(t => t.Email == Email);
            if (string.IsNullOrEmpty(Email) || _context.Traveller.Any(t => t.Email == Email) == false)
            {
                return RedirectToPage("/Travellers/MyDetails");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check for availability
            var isAvailable = !_context.Booking.Any(b =>
                b.ApartmentID == Booking.ApartmentID &&
                ((Booking.CheckIn >= b.CheckIn && Booking.CheckIn < b.CheckOut) ||
                 (Booking.CheckOut > b.CheckIn && Booking.CheckOut <= b.CheckOut) ||
                 (Booking.CheckIn <= b.CheckIn && Booking.CheckOut >= b.CheckOut))
            );

            if (isAvailable)
            {
                // Calculate the cost
                var apartment = await _context.Apartment
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.ID == Booking.ApartmentID);
                if (apartment == null)
                {
                    ModelState.AddModelError(string.Empty, "Apartment not found.");
                    return Page();
                }

                int totalNights = (Booking.CheckOut - Booking.CheckIn).Days;
                decimal cost = apartment.Price * totalNights;

                string Email = User.FindFirst(ClaimTypes.Name).Value;
                var bookingEntity = new Booking
                {
                    TravellerEmail = Email,
                    ApartmentID = Booking.ApartmentID,
                    CheckIn = Booking.CheckIn,
                    CheckOut = Booking.CheckOut,
                    Cost = cost
                };

                _context.Booking.Add(bookingEntity);
                await _context.SaveChangesAsync();

                BookingInserted = true;
                ApartmentLevel = apartment.Level;
                BookingCost = cost;
                IsBookingAvailable = true;
                return Page();
            }
            else
            {
                IsBookingAvailable = false;
                return Page();
            }
        }

    }

    public class BookingViewModel
    {
        [Required, Range(1, 16)]
        public int ApartmentID { get; set; }

        [Required, DataType(DataType.Date)]
        [DateNotInThePast(ErrorMessage = "Check-in date must not be earlier than the current date.")]
        public DateTime CheckIn { get; set; }

        [Required, DataType(DataType.Date)]
        [DateGreaterThan("CheckIn", ErrorMessage = "Check-out date must be later than the check-in date.")]
        public DateTime CheckOut { get; set; }
    }
}
