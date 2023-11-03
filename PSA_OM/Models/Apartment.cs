using System.ComponentModel.DataAnnotations;

namespace PSA_OM.Models
{
    public class Apartment
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please specify an apartment level!")]
        public String Level { get; set; } // follwing project requirements, but wouldn't a char be better?
        [Range(1, 3)]
        public int BedroomCount { get; set; }
        [Range(200,500)]
        public decimal Price { get; set; }

        //public ICollection<Booking> TheBookings { get; set; }
    }
}
