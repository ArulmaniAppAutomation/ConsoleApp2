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
        public static List<RootObject> TestCases { get; private set; }

        // Load JSON data from file once
        public static void LoadFromFile(string jsonFilePath)
        {
            var json = File.ReadAllText(jsonFilePath);
            TestCases = JsonSerializer.Deserialize<List<RootObject>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }

}
