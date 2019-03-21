using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SquashNiagara.Models
{
    public class Division
    {
        public Division()
        {
            SeasonDivisionTeams = new HashSet<SeasonDivisionTeam>();
            Fixtures = new HashSet<Fixture>();
            PlayerRankings = new HashSet<PlayerRanking>();
        }

        public int ID { get; set; }

        [Required(ErrorMessage = "You cannot leave the Division Name blank.")]
        [Display(Name = "Division Name")]
        [StringLength(50, ErrorMessage = "Division name cannot be more than 50 characters long.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You have to choose the number of positions.")]
        [Display(Name = "Number of Positions")]
        [Range(1, 5, ErrorMessage = "Position has to be beteween 1 and 5")]
        [RegularExpression("^\\d{1}$", ErrorMessage = "Please enter a valid number")]
        public Int16 PositionNo { get; set; }

        public ICollection<SeasonDivisionTeam> SeasonDivisionTeams { get; set; }

        public ICollection<Fixture> Fixtures { get; set; }
        public virtual ICollection<PlayerRanking> PlayerRankings { get; set; }

        public static implicit operator short(Division v)
        {
            throw new NotImplementedException();
        }
    }
}
