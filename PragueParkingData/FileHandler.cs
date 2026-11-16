using System.Text.Json;

namespace PragueParkingData
{
    public class FileHandler
    {
        public void Spara<T>(T data, string pHuset)
        {
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(pHuset, json);
        }

        public T? Läs<T>(string pHuset)
        {
            if (!File.Exists(pHuset))
            {
                return default;
            }
            string json = File.ReadAllText(pHuset);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
