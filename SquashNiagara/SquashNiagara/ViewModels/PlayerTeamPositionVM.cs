using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SquashNiagara.Models;

namespace SquashNiagara.ViewModels
{
    public class PlayerTeamPositionVM
    {
        public Player Player { get; set; }
        public int Team { get; set; }
        public int Position { get; set; }
    }
}
