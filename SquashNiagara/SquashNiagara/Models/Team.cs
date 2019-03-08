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
            //PlayerTeams = new HashSet<PlayerTeam>();
            Players = new HashSet<Player>();
            HomeFixtures = new HashSet<Fixture>();
            AwayFixtures = new HashSet<Fixture>();
        }

        public int ID { get; set; }

        [Required(ErrorMessage = "You cannot leave the Team Name blank.")]
        [Display(Name = "Team Name")]
        [StringLength(50, ErrorMessage = "Team name cannot be more than 50 characters long.")]
        public string Name { get; set; }

        [Display(Name = "Captain")]
        public int? CaptainID { get; set; }
        public virtual Player Captain { get; set; }

        [Display(Name = "Venue")]
        public int? VenueID { get; set; }
        public virtual Venue Venue { get; set; }

        [ScaffoldColumn(false)]
        public byte[] imageContent { get; set; }

        [StringLength(256)]
        [ScaffoldColumn(false)]
        public string imageMimeType { get; set; }

        [Display(Name = "File Name")]
        [StringLength(100, ErrorMessage = "File name too long")]
        [ScaffoldColumn(false)]
        public string imageFileName { get; set; }

        [Display(Name = "Profile")]
        [StringLength(1000, ErrorMessage = "Profile cannot be more than 1000 characters long.")]
        public string Profile { get; set; }

        public ICollection<SeasonDivisionTeam> SeasonDivisionTeams { get; set; }

        //public virtual ICollection<PlayerTeam> PlayerTeams { get; set; }
        public virtual ICollection<Player> Players { get; set; }

        public virtual ICollection<Fixture> HomeFixtures { get; set; }

        public virtual ICollection<Fixture> AwayFixtures { get; set; }

    }
}
