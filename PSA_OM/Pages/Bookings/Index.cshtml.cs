﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PSA_OM.Data;
using PSA_OM.Models;

namespace PSA_OM.Pages.Bookings
{
    public class IndexModel : PageModel
    {
        private readonly PSA_OM.Data.ApplicationDbContext _context;

        public IndexModel(PSA_OM.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Booking> Booking { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Booking != null)
            {
                Booking = await _context.Booking
                .Include(b => b.TheRoom)
                .Include(b => b.TheTraveller).ToListAsync();
            }
        }
    }
}
