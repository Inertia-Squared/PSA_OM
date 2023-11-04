using System.ComponentModel.DataAnnotations;

namespace PSA_OM.Models
{
    public class Booking
    {
        public int ID { get; set; }
        public int ApartmentID { get; set; }
        public required string TravellerEmail { get; set; }
        [Display(Name = "Check-in Date")]
        [DataType(DataType.Date)]
        public DateTime CheckIn { get; set; }
        [Display(Name = "Check-out Date")]
        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }
        [Range(0, 10000)]
        public decimal Cost { get; set; }

        public Apartment? TheRoom { get; set; }
        public Traveller? TheTraveller { get; set; }

    }
}
