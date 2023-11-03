using System.ComponentModel.DataAnnotations;

namespace PSA_OM.Models
{
    public class Booking
    {
        public int ID { get; set; }
        public int ApartmentID { get; set; }
        public string TravellerEmail { get; set; }
        [DataType(DataType.Date)]
        public DateTime CheckIn { get; set; }
        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }
        [Range(0, 10000)]
        public decimal Cost { get; set; }

        public Apartment? TheRoom { get; set; }
        public Traveller? TheTraveller { get; set; }

    }
}
