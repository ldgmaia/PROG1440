using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SquashNiagara.Models
{
    public class TeamRanking
    {
        public int ID { get; set; }

        public int TeamID { get; set; }
        public virtual Team Team { get; set; }

        public int DivisionID { get; set; }
        public virtual Division Division { get; set; }

        public int SeasonID { get; set; }
        public virtual Season Season { get; set; }

        [DisplayFormat(DataFormatString = "{0:N1}")]
        public double Points { get; set; }

        public int Played { get; set; }

        public int Won { get; set; }

        public int Lost { get; set; }

        public int Strength { get; set; }

    }
}
