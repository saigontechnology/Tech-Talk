using DapperSharing.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DapperSharing.Utils
{
    public static class DisplayHelper
    {
        public static void PrintJson (object value)
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
        public static void PrintListOfMethods(Type classType)
        {
            Console.WriteLine("List of methods:");
            var countMethod = 1;
            foreach (var method in classType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
            {
                Console.WriteLine($"{countMethod}. {method.Name}");
                countMethod++;
            }
        }

        public static void PrintAllClassNames(string myNameSpace)
        {
            var classNamesList = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.IsClass && t.IsPublic && t.Namespace == myNameSpace)
                    .Select(t => t.Name);
            foreach (var className in classNamesList)
            {
                Console.WriteLine(className);
            }
        }

    }
}
