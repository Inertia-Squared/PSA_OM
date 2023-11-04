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

            var numberOfBedrooms = SearchModel.NumberOfBedrooms;
            var checkInDate = SearchModel.CheckInDate;
            var checkOutDate = SearchModel.CheckOutDate;

            var sqlQuery = @"
            SELECT a.*
            FROM Apartments AS a
            WHERE a.BedroomCount = {0}
              AND NOT EXISTS (
                SELECT 1
                FROM Bookings AS b
                WHERE b.TheRoomID = a.ID
                  AND {1} < b.CheckOut
                  AND b.CheckIn < {2}
              )";

            AvailableApartments = await _context.Apartment
                .FromSqlRaw(sqlQuery, numberOfBedrooms, checkInDate, checkOutDate)
                .ToListAsync();

            return Page();
        }

    }

}
