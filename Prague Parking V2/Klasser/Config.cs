using System;
using System.Collections.Generic;
using System.Text;

namespace Prague_Parking_V2.Klasser
{
    public class Config
    {
        public int AntalParkeringsrutor { get; set; }
        public int ParkeringsrutaKapacitet { get; set; }
        public List<FordonstypConfig> Fordonstyper { get; set; }

        public Config()
        {
            AntalParkeringsrutor = 100;
            ParkeringsrutaKapacitet = 4;
            Fordonstyper = new List<FordonstypConfig>
            {
                new FordonstypConfig { Fordonstyp = "BIL", Fordonsstorlek = 4, MaxAntalPerPlats = 1 },
                new FordonstypConfig { Fordonstyp = "MC", Fordonsstorlek = 2, MaxAntalPerPlats = 2 }
            };
        }
    }
}
