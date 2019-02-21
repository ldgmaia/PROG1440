using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SquashNiagara.Models
{
    public class Team
    {
        public Team()
        {
            SeasonDivisionTeams = new HashSet<SeasonDivisionTeam>();
            PlayerTeams = new HashSet<PlayerTeam>();
            HomeFixtures = new HashSet<Fixture>();
            AwayFixtures = new HashSet<Fixture>();
        }

        public int ID { get; set; }

        [Required(ErrorMessage = "You cannot leave the Team Name blank.")]
        [Display(Name = "Team Name")]
        [StringLength(50, ErrorMessage = "Team name cannot be more than 50 characters long.")]
        public string Name { get; set; }

        [Display(Name = "Captain")]
        public int CaptainID { get; set; }
        public virtual Player Captain { get; set; }

        [Display(Name = "Venue")]
        public int VenueID { get; set; }
        public virtual Venue Venue { get; set; }
              
        public ICollection<SeasonDivisionTeam> SeasonDivisionTeams { get; set; }

        public virtual ICollection<PlayerTeam> PlayerTeams { get; set; }

        public virtual ICollection<Fixture> HomeFixtures { get; set; }

        public virtual ICollection<Fixture> AwayFixtures { get; set; }

    }
}
