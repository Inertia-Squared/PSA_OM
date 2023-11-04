using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PSA_OM.Models;

namespace PSA_OM.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Apartment> Apartment { get; set; } = default!;
        public DbSet<Booking> Booking { get; set; } = default!;
        public DbSet<Traveller> Traveller { get; set; } = default!;
    }
}