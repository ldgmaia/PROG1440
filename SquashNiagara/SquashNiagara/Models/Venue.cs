using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SquashNiagara.Models
{
    public class Venue
    {
        public Venue()
        {
            Teams = new HashSet<Team>();
            Fixtures = new HashSet<Fixture>();
        }

        public int ID { get; set; }

        [Required(ErrorMessage = "You cannot leave the Venue Name blank.")]
        [Display(Name = "Venue Name")]
        [StringLength(50, ErrorMessage = "Position name cannot be more than 50 characters long.")]
        public string Name { get; set; }

        [Display(Name = "Address")]
        [StringLength(256, ErrorMessage = "Address cannot be more than 50 characters long.")]
        public string Address { get; set; }

        [Display(Name = "City")]
        [StringLength(256, ErrorMessage = "City cannot be more than 50 characters long.")]
        public string City { get; set; }

        [Display(Name = "Province")]
        [StringLength(2)]
        public string Province { get; set; }

        [Display(Name = "Postal Code")]
        [RegularExpression("^\\d{6}$", ErrorMessage = "The Postal Code must be exactly 6 characteres.")]
        [StringLength(6)]
        public string PostalCode { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        public virtual ICollection<Fixture> Fixtures { get; set; }
    }
}
