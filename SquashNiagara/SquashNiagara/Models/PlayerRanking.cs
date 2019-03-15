using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SquashNiagara.Models
{
    public class PlayerRanking
    {
        //public PlayerRanking(PlayerRanking playerRanking)
        //{
        //}

        //public PlayerRanking(PlayerRanking playerRanking)
        //{
        //    Played = 0;
        //}

        public int ID { get; set; }

        public int PlayerID { get; set; }
        public virtual Player Player { get; set; }
        
        public int SeasonID { get; set; }
        public virtual Season Season { get; set; }

        public int DivisionID { get; set; }
        public virtual Division Division { get; set; }

        [DisplayFormat(DataFormatString = "{0:N1}")]
        public double Average { get; set; }

        public int Played { get; set; }

        public int WonMatches { get; set; }

        public int LostMatches { get; set; }

        public int WonGames { get; set; }

        public int LostGames { get; set; }

        [DisplayFormat(DataFormatString = "{0:N1}")]
        public double Points { get; set; }



    }
}
