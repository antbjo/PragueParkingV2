using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Prague_Parking_V2.Klasser
{
    [JsonDerivedType(typeof(Bil), "CAR")]
    [JsonDerivedType(typeof(MC), "MC")]
    public abstract class Fordon
    {
        public string RegNr { get; set; }
        public string FordonsTyp { get; set; }
        public abstract int Storlek { get; }
        public abstract decimal TimPris { get; }
        public DateTime StartTid { get; set; }
        public abstract void AngeRegNr(Parkeringshus pHus);
        public virtual string SättIhopRegNrOchFordonstyp()
        {
            return string.Join('#', FordonsTyp, RegNr.ToUpper());
        }
        protected bool ÄrRegNrKorrekt(string regNr)
        {
            if (string.IsNullOrEmpty(regNr) || regNr.Length > 10)
            {
                return false;
            }
            foreach (var tecken in regNr)
            {
                if (!char.IsLetterOrDigit(tecken))
                {
                    return false;
                }
            }
            return true;
        }

        public TimeSpan HämtaTidParkerad()
        {
            return DateTime.Now - StartTid;
        }

    }

}
