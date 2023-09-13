using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DapperSharing.Helper
{
    public static class DisplayHelper
    {
        public static void PrintJson(object value)
        {
            Console.WriteLine(JsonSerializer.Serialize(value, new JsonSerializerOptions
            {
                WriteIndented = true
            }));
        }

        public static void PrintJsonWithoutLoop(object value)
        {
            Console.WriteLine(JsonSerializer.Serialize(value, new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            }));
        }
    }
}
