using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SquashNiagara.Models
{
    public class Position
    {
        public Position()
        {
            //PlayerTeams = new HashSet<PlayerTeam>();
            PlayerPositions = new HashSet<PlayerPosition>();
        }

        public int ID { get; set; }

        [Required(ErrorMessage = "You cannot leave the Position Name blank.")]
        [Display(Name = "Position Name")]
        [StringLength(10, ErrorMessage = "Position name cannot be more than 10 characters long.")]
        public string Name { get; set; }

        //public virtual ICollection<PlayerTeam> PlayerTeams { get; set; }

        public virtual ICollection<PlayerPosition> PlayerPositions { get; set; }
    }
}
