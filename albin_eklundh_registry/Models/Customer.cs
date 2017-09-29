using System;
using System.ComponentModel.DataAnnotations;

namespace albin_eklundh_registry.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Du måste skriva i kontaktpersonen")]
        [MaxLength(100, ErrorMessage = "Kontaktpersonens namn får inte överskrida 100 tecken")]
        public string ContactPerson { get; set; }

        public bool IsCompany { get; set; }

        [MaxLength(200, ErrorMessage = "Företagsnamnet får inte överskrida 200 tecken")]
        public string CompanyName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Du måste ange en gatuaddress")]
        [MaxLength(100, ErrorMessage = "Gatuaddressen får inte överskrida 100 tecken")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Du måste ange ort")]
        [MaxLength(50, ErrorMessage = "Ortens namn får inte överskrida 50 tecken")]
        public string Area { get; set; }

        [Required(ErrorMessage = "Du måste ange postkod")]
        [RegularExpression(@"^\d{5}", ErrorMessage = "Postkoden måste anges med fem siffror (ex. 14141)")]
        public string PostalCode { get; set; }

        public string Phone { get; set; }

        [Required(ErrorMessage = "Du måste ange en e-postaddress")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Du måste ange en e-postaddress")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Du måste ange om kunden vill ha nyhetsbrev")]
        public bool WantsNewsletter { get; set; }

        public string Notes { get; set; }
    }
}
