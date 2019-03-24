using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SquashNiagara.Models
{
    public class Match
    {
        public Match()
        {
            PlayerPositions = new HashSet<PlayerPosition>();
        }

        public int ID { get; set; }

        [Display(Name = "Fixture")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Fixture to create the matches.")]
        public int FixtureID { get; set; }
        public virtual Fixture Fixture { get; set; }

        [Display(Name = "Home Player")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a player to create the matches.")]
        public int HomePlayerID { get; set; }
        public virtual Player HomePlayer { get; set; }

        [Display(Name = "Away Player")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a player to create the matches.")]
        public int AwayPlayerID { get; set; }
        public virtual Player AwayPlayer { get; set; }

        [Display(Name = "Home Player Score")]
        [Range(0, 3, ErrorMessage = "Score have to be 0 or 3")]
        [RegularExpression("^\\d{1}$", ErrorMessage = "Please enter a valid number")]
        public Int16? HomePlayerScore { get; set; }

        [Display(Name = "Away Player Score")]
        [Range(0, 3, ErrorMessage = "Score have to be 0 or 3")]
        [RegularExpression("^\\d{1}$", ErrorMessage = "Please enter a valid number")]
        public Int16? AwayPlayerScore { get; set; }

        [Display(Name = "Match Position")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a player to create the matches.")]
        public int? PositionID { get; set; }
        public virtual Position Position { get; set; }

        public virtual ICollection<PlayerPosition> PlayerPositions { get; set; }
    }
}
