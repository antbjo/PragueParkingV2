using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace Prague_Parking_V2.Klasser
{
    public class FileHandler
    {
        public void SparaParkeringsHus(Parkeringshus pHus)
        {
            string json = JsonSerializer.Serialize(pHus, new JsonSerializerOptions { WriteIndented = true });      
            File.WriteAllText("pHuset.json", json);
        }
        public Parkeringshus? LäsParkeringsHus()
        {
            if (!File.Exists("pHuset.json"))
            {
                return null;
            }
            string json = File.ReadAllText("pHuset.json");
            return JsonSerializer.Deserialize <Parkeringshus>(json);
        }
        public void SparaConfig(Config config)
        {
            string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("config.json", json);
        }
        public Config LäsConfig()
        {
            if (!File.Exists("config.json"))
            {
                return null;
            }
            string json = File.ReadAllText("config.json");
            return JsonSerializer.Deserialize<Config>(json);
        }
    }
}
