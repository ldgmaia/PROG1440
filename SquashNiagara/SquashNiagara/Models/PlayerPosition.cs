using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquashNiagara.Models
{
    public class PlayerPosition
    {
        public int PlayerID { get; set; }
        public virtual Player Player { get; set; }

        //public int MatchID { get; set; }
        //public virtual Match Match { get; set; }

        public int PositionID { get; set; }
        public virtual Position Position { get; set; }

    }
}
