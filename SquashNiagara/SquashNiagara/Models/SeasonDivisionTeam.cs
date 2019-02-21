using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquashNiagara.Models
{
    public class SeasonDivisionTeam
    {

        public int SeasonID { get; set; }
        public Season Season { get; set; }

        public int DivisionID { get; set; }
        public Division Division { get; set; }

        public int TeamID { get; set; }
        public Team Team { get; set; }
    }
}
