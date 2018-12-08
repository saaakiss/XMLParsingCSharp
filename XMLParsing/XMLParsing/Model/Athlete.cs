using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLParsing.Model
{
    public class Athlete
    {
        public string Label { get; set; }

        public string Number { get; set; }

        public string FullName { get; set; }

        public string Nationality { get; set; }

        public string Position { get; set; }

        public string Height { get; set; }

        public string Birthday { get; set; }

        public string ImgUrl { get; set; }

        public Achievements AthlAchievements { get; set; }

        public Athlete(Achievements AthlAchievements)
        {
            this.AthlAchievements = AthlAchievements;
        }


    }
}
