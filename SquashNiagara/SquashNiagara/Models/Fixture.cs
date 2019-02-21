using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SquashNiagara.Models
{
    public class Fixture
    {
        public Fixture()
        {
            Matches = new HashSet<Match>();
        }

        public int ID { get; set; }

        [Display(Name = "Season")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select the Division.")]
        public int SeasonID { get; set; }
        public Season Season { get; set; }

        [Display(Name = "Division")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select the Division.")]
        public int DivisionID { get; set; }
        public Division Division { get; set; }

        [Display(Name = "Home Team")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select the Home Team.")]
        public int HomeTeamID { get; set; }
        public Team HomeTeam { get; set; }

        [Display(Name = "Away Team")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select the Away Team.")]
        public int AwayTeamID { get; set; }
        public Team AwayTeam { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Time is required")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [Display(Name = "Venue")]
        public int VenueID { get; set; }
        public Venue Venue { get; set; }

        [Display(Name = "Home Team Score")]
        [Range(0, 5, ErrorMessage = "Score have to be beteween 0 and 5")]
        [RegularExpression("^\\d{1}$", ErrorMessage = "Please enter a valid number")]
        public Int16? HomeTeamScore { get; set; }

        [Display(Name = "Away Team Score")]
        [Range(0, 5, ErrorMessage = "Score have to be beteween 0 and 5")]
        [RegularExpression("^\\d{1}$", ErrorMessage = "Please enter a valid number")]
        public Int16? AwayTeamScore { get; set; }

        [Display(Name = "Home Team Bonus")]
        [Range(0, 1, ErrorMessage = "Bonus have to be 0 or 1")]
        [RegularExpression("^\\d{1}$", ErrorMessage = "Please enter a valid number")]
        public Int16? HomeTeamBonus { get; set; }

        [Display(Name = "Away Team Bonus")]
        [Range(0, 1, ErrorMessage = "Bonus have to be 0 or 1")]
        [RegularExpression("^\\d{1}$", ErrorMessage = "Please enter a valid number")]
        public Int16? AwayTeamBonus { get; set; }

        [ScaffoldColumn(false)]
        public int? CaptainResultID { get; set; }
        public virtual Player CaptainResult { get; set; }

        [ScaffoldColumn(false)]
        public int? CaptainApproveID { get; set; }
        public virtual Player CaptainApprove { get; set; }

        [ScaffoldColumn(false)]
        public bool? Approved { get; set; }

        public virtual ICollection<Match> Matches { get; set; }
    }
}
