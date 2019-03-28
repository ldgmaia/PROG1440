using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquashNiagara.ViewModels
{
    public class PositionalStandings
    {
        public int PlayerID { get; set; }
        public int PositionID { get; set; }
        public string PlayerName { get; set; }
        public decimal PercPlayed { get; set; }
        public decimal PositionAverage { get; set; }
    }
}
