using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PSA_OM.Data;
using PSA_OM.Models;

namespace PSA_OM.Pages.Bookings
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly PSA_OM.Data.ApplicationDbContext _context;

        public IndexModel(PSA_OM.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Booking> Booking { get; set; }
        public string CurrentSort { get; set; }

        public async Task OnGetAsync(string sortOrder)
        {
            CurrentSort = sortOrder;

            IQueryable<Booking> bookingIQ = from b in _context.Booking
                                            .Include(b => b.TheRoom)
                                            .Include(b => b.TheTraveller)
                                            select b;

            if (User.Identity.IsAuthenticated)
            {
                string Email = User.FindFirst(ClaimTypes.Name).Value;
                bookingIQ = bookingIQ.Where(b => b.TravellerEmail == Email);
            }

            switch (sortOrder)
            {
                case "checkin":
                    bookingIQ = bookingIQ.OrderBy(b => b.CheckIn);
                    break;
                case "checkin_desc":
                    bookingIQ = bookingIQ.OrderByDescending(b => b.CheckIn);
                    break;
                case "cost":
                    bookingIQ = bookingIQ.OrderBy(b => (double)b.Cost);
                    break;
                case "cost_desc":
                    bookingIQ = bookingIQ.OrderByDescending(b => (double)b.Cost);
                    break;
                default:
                    break;
            }

            Booking = await bookingIQ.ToListAsync();
        }
    }
}
