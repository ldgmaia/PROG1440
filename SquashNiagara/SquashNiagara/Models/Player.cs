using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SquashNiagara.Models
{
    public class Player
    {
        public Player()
        {
            TeamCaptains = new HashSet<Team>();
            PlayerTeams = new HashSet<PlayerTeam>();
            FixtureCaptainResults = new HashSet<Fixture>();
            FixtureCaptainApproves = new HashSet<Fixture>();
            HomeMatches = new HashSet<Match>();
            AwayMatches = new HashSet<Match>();
            PlayerPositions = new HashSet<PlayerPosition>();
        }

        public int ID { get; set; }

                
        [Required(ErrorMessage = "You cannot leave the first name blank.")]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First name cannot be more than 50 characters long.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "You cannot leave the last name blank.")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last name cannot be more than 50 characters long.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        [Display(Name = "E-Mail")]
        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; }

        [Display(Name ="Teams Captains")]
        public virtual ICollection<Team> TeamCaptains { get; set; }

        public virtual ICollection<PlayerTeam> PlayerTeams { get; set; }

        public virtual ICollection<Fixture> FixtureCaptainResults { get; set; }

        public virtual ICollection<Fixture> FixtureCaptainApproves { get; set; }

        public virtual ICollection<Match> HomeMatches { get; set; }

        public virtual ICollection<Match> AwayMatches { get; set; }

        [Display(Name = "Players Positions")]
        public virtual ICollection<PlayerPosition> PlayerPositions { get; set; }
    }
}
