﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PSA_OM.Data;
using PSA_OM.Models;

namespace PSA_OM.Pages.Bookings
{
    [Authorize(Roles = "travellers")]
    public class IndexModel : PageModel
    {
        private readonly PSA_OM.Data.ApplicationDbContext _context;

        public IndexModel(PSA_OM.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Booking> Booking { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Booking != null)
            {
                string Email = User.FindFirst(ClaimTypes.Name).Value;
                Booking = await _context.Booking
                    .Include(b => b.TheRoom)
                    .Include(b => b.TheTraveller)
                    .Where(b => b.TravellerEmail == Email)
                    .ToListAsync();
            }
        }
    }
}
