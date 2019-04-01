using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SquashNiagara.Models;

namespace SquashNiagara.ViewModels
{
    public class FixtureMatchVM
    {
        public FixtureMatchVM()
        {
            this.Matches = new List<Match>();
        }
        public Fixture Fixture { get; set; }
        public List<Match> Matches { get; set; }
    }
}
