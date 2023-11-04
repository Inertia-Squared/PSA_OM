using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSA_OM.Models
{
    public class Traveller
    {
        [Key, Required, EmailAddress]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Email { get; set; }
        [Required, RegularExpression(@"[a-zA-Z-']+"), MinLength(2), MaxLength(20)]
        public string Surname { get; set; }
        [Required, RegularExpression(@"[a-zA-Z-']+"), MinLength(2), MaxLength(20)]
        [Display(Name = "Given Name")]
        public string GivenName { get; set; }
        [Required, RegularExpression(@"[0-9]{4}")]
        public string Postcode { get; set; }

        [NotMapped]
        public string FullName => $"{GivenName} {Surname}";

        public ICollection<Booking>? TheBookings { get; set; }
    }
}
