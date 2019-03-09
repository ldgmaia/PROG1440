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
            //TeamCaptains = new HashSet<Team>();
            //PlayerTeams = new HashSet<PlayerTeam>();
            FixtureCaptainResults = new HashSet<Fixture>();
            FixtureCaptainApproves = new HashSet<Fixture>();
            HomeMatches = new HashSet<Match>();
            AwayMatches = new HashSet<Match>();
            //Positions = new HashSet<Position>();
        }

        [Display(Name = "Player")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
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

        [ScaffoldColumn(false)]
        public byte[] imageContent { get; set; }

        [StringLength(256)]
        [ScaffoldColumn(false)]
        public string imageMimeType { get; set; }

        [Display(Name = "File Name")]
        [StringLength(100, ErrorMessage = "File name too long")]
        [ScaffoldColumn(false)]
        public string imageFileName { get; set; }

        //[Display(Name = "Players Team")]
        //[Range(1, int.MaxValue, ErrorMessage = "You must select a Team!")]
        //public virtual ICollection<PlayerTeam> PlayerTeams { get; set; }

        public int? TeamID { get; set; }
        public virtual Team Team { get; set; }

        public int? PositionID { get; set; }
        public virtual Position Position { get; set; }

        

        public virtual ICollection<Fixture> FixtureCaptainResults { get; set; }

        public virtual ICollection<Fixture> FixtureCaptainApproves { get; set; }

        public virtual ICollection<Match> HomeMatches { get; set; }

        public virtual ICollection<Match> AwayMatches { get; set; }

    }
}