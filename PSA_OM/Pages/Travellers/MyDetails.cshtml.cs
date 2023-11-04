using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PSA_OM.Models;
using System.Security.Claims;

namespace PSA_OM.Pages.Travellers
{
        [Authorize(Roles = "travellers, managers")]
        public class MyDetailsModel : PageModel
        {
            private readonly Data.ApplicationDbContext _context;

            public MyDetailsModel(Data.ApplicationDbContext context)
            {
                _context = context;
            }

            [BindProperty]
            public Traveller? Myself { get; set; }

            public async Task<IActionResult> OnGetAsync()
            {
                string _Email = User.FindFirst(ClaimTypes.Name).Value;

                Traveller traveller = await _context.Traveller.FirstOrDefaultAsync(m => m.Email == _Email);

                Console.WriteLine(traveller is not null);

                if (traveller is not null)
                {
                    ViewData["ExistInDB"] = "true";
                    Myself = traveller;
                }
                else
                {
                    ViewData["ExistInDB"] = "false";
                }

                return Page();
            }

            public async Task<IActionResult> OnPostAsync()
            {
                string _Email = User.FindFirst(ClaimTypes.Name).Value;

                Traveller traveller = await _context.Traveller.FirstOrDefaultAsync(m => m.Email == _Email);

                if (traveller is not null)
                {
                    ViewData["ExistInDB"] = "true";
                }
                else
                {
                    ViewData["ExistInDB"] = "false";
                    traveller = new Traveller();
                }
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                traveller.Email = _Email;
                var success = await TryUpdateModelAsync<Traveller>(traveller, "Myself", s => s.GivenName, s => s.Surname, s => s.Postcode);
                success.ToString();
                if (!success)
                {
                    return Page();
                }

                if (ViewData["ExistInDB"] as string == "true")
                    _context.Traveller.Update(traveller);
                else _context.Traveller.Add(traveller);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                ViewData["SuccessDB"] = "success";
                return Page();
        }
    }
}
