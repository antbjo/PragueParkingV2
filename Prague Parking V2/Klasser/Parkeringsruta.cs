using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Prague_Parking_V2.Klasser
{
    public class Parkeringsruta
    {
        public int Platsnummer { get; set; }
        public int Kapacitet { get; set; }
        public Parkeringsruta()
        {
            
        }

        public Parkeringsruta(int platsnummer, int kapacitet)
        {
            Platsnummer = platsnummer;
            Kapacitet = kapacitet;
        }

        public List<Fordon> ParkeradeFordon { get; set; } = new();

    }
}
