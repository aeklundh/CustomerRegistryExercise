using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace albin_eklundh_registry.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ContactPerson { get; set; }

        public bool IsCompany { get; set; }

        [MaxLength(200)]
        public string CompanyName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Required]
        [MaxLength(50)]
        public string Address { get; set; }

        [Required]
        [MaxLength(50)]
        public string Area { get; set; }

        [Required]
        [RegularExpression(@"^\d{5}")]
        public string PostalCode { get; set; }

        public string Phone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public bool WantsNewsletter { get; set; }

        public string Notes { get; set; }
    }
}
