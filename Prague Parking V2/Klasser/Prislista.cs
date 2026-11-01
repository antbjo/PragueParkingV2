using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Prague_Parking_V2.Klasser
{
    public class Prislista
    {
        public decimal BilPris { get; set; }
        public decimal MCPris { get; set; }

        public void LäsPrislista()
        {
            string[] lines = File.ReadAllLines("prislista.txt");
            foreach (var line in lines)
            {
                string[] split = line.Split('=');

                if (split[0] == "BIL")
                {
                    BilPris = decimal.Parse(split[1]);
                }
                else if (split[0] == "MC")
                {
                    MCPris = decimal.Parse(split[1]);
                }
            }
        }
    }
}
