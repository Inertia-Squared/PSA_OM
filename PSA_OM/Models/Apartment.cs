using System.ComponentModel.DataAnnotations;

namespace PSA_OM.Models
{
    public class Apartment
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please specify an apartment level!")]
        [MaxLength(1), Display(Name = "Apartment Level")]
        public String Level { get; set; } // following project requirements, but wouldn't a char be better here?
        [Range(1, 3)]
        [Display(Name = "Bedroom(s)")]
        public int BedroomCount { get; set; }
        [Range(200,500)]
        public decimal Price { get; set; }

        public ICollection<Booking>? TheBookings { get; set; }
    }
}
