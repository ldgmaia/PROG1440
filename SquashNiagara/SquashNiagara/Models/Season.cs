using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SquashNiagara.Models
{
    public class Season
    {
        public Season()
        {
            SeasonDivisionTeams = new HashSet<SeasonDivisionTeam>();
            Fixtures = new HashSet<Fixture>();
            PlayerRankings = new HashSet<PlayerRanking>();
        }

        public int ID { get; set; }

        [Required(ErrorMessage = "You cannot leave the Season Name blank.")]
        [Display(Name = "Season Name")]
        [StringLength(50, ErrorMessage = "Season name cannot be more than 50 characters long.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You cannot leave the Start Date blank.")]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "You cannot leave the End Date blank.")]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public ICollection<SeasonDivisionTeam> SeasonDivisionTeams { get; set; }
        public ICollection<Fixture> Fixtures { get; set; }
        public virtual ICollection<PlayerRanking> PlayerRankings { get; set; }

    }
}
