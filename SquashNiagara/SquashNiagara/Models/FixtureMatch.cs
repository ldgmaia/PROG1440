using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquashNiagara.Models
{
    public class FixtureMatch
    {
        public Fixture Fixture { get; set; }
        public List<Match> Matches { get; set; }
    }
}
