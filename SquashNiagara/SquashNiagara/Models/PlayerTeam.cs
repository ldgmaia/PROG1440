using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquashNiagara.Models
{
    public class PlayerTeam
    {
        public int PlayerID { get; set; }
        public Player Player { get; set; }

        public int TeamID { get; set; }
        public Team Team { get; set; }

        public int PositionID { get; set; }
        public Position Position { get; set; }
    }
}
