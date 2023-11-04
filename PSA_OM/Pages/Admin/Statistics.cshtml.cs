using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PSA_OM.Data;
using PSA_OM.Models;

namespace PSA_OM.Pages.Manager
{
    [Authorize(Roles = "managers")]
    public class StatisticsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public StatisticsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Dictionary<string, int> TravellerCounts { get; set; } = new Dictionary<string, int>();
        public Dictionary<int, int> BookingCounts { get; set; } = new Dictionary<int, int>();
        public Dictionary<int, int> BedroomBookingCounts { get; set; } = new Dictionary<int, int>();

        public async Task OnGetAsync()
        {
            TravellerCounts = await _context.Traveller
                .GroupBy(t => t.Postcode)
                .Select(g => new { Postcode = g.Key, Count = g.Count() })
                .ToDictionaryAsync(t => t.Postcode, t => t.Count);

            BookingCounts = await _context.Booking
                .Include(b => b.TheRoom)
                .GroupBy(b => b.TheRoom.ID)
                .Select(g => new { ApartmentId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(b => b.ApartmentId, b => b.Count);

            BedroomBookingCounts = await _context.Booking
                .Include(b => b.TheRoom)
                .GroupBy(b => b.TheRoom.BedroomCount)
                .Select(g => new { BedroomCount = g.Key, Count = g.Count() })
                .ToDictionaryAsync(b => b.BedroomCount, b => b.Count);
        }
    }
}
