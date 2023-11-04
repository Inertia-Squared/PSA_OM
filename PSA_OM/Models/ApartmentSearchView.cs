using PSA_OM.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace PSA_OM.Models
{
    public class ApartmentSearchView
    {
        [Required(ErrorMessage = "Please select the number of bedrooms.")]
        [Range(1, 3, ErrorMessage = "Number of bedrooms must be between 1 and 3.")]
        [Display(Name = "Number of Bedrooms")]
        public int NumberOfBedrooms { get; set; }

        [Required(ErrorMessage = "Please select a check-in date.")]
        [DataType(DataType.Date)]
        [Display(Name = "Check-In Date")]
        [DateNotInThePast(ErrorMessage = "Check-in date must not be earlier than the current date.")] // using custom validation attribute here
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Please select a check-out date.")]
        [DataType(DataType.Date)]
        [Display(Name = "Check-Out Date")]
        [DateGreaterThan("CheckInDate", ErrorMessage = "Check-out date must be later than the check-in date.")] // and here
        public DateTime CheckOutDate { get; set; }
    }
}
