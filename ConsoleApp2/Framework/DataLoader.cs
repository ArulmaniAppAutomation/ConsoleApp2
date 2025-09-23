using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.Framework
{
    using System.Text.Json;

    public static class DataLoader
    {
        public static List<RootObject> LoadFromFile(string jsonFilePath)
        {
            var json = File.ReadAllText(jsonFilePath);
            return JsonSerializer.Deserialize<List<RootObject>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new();
        }
    }

}
