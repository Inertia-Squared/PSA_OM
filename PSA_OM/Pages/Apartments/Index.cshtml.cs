using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PSA_OM.Data;
using PSA_OM.Models;

namespace PSA_OM.Pages.Apartments
{
    [Authorize(Roles = "travellers, managers")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Apartment> AvailableApartments { get; set; } = new List<Apartment>();

        [BindProperty]
        public ApartmentSearchView SearchModel { get; set; } = new ApartmentSearchView();

        public void OnGet()
        {
            SearchModel.CheckInDate = DateTime.Today;
            SearchModel.CheckOutDate = DateTime.Today.AddDays(1);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            AvailableApartments = await _context.Apartment
            .Where(a => a.BedroomCount == SearchModel.NumberOfBedrooms && !_context.Booking
                .Where(b => SearchModel.CheckInDate < b.CheckOut && b.CheckIn < SearchModel.CheckOutDate)
                .Select(b => b.TheRoom.ID)
                .Contains(a.ID))
            .ToListAsync();
            return Page();
        }
    }

}
