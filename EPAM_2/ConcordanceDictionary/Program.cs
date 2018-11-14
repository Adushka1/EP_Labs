using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using ConcordanceDictionary.TextAnalyzer;
using ConcordanceDictionary.TextAnalyzer.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConcordanceDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            var readPath = ConfigurationManager.AppSettings["ReadPath"];
            var outputPath = ConfigurationManager.AppSettings["OutputPath"];
            try
            {
                JsonSerializer jsonSerializer = new JsonSerializer();
                jsonSerializer.Converters.Add(new KeyValuePairConverter());
                IConcordance concordance = new Concordance();
                using (StreamReader sr = new StreamReader(readPath))
                {
                    var i = 1;
                    while (sr.Peek() >= 0)
                    {
                        var line = sr.ReadLine();
                        concordance.GetWordInfos(line, i);
                        i++;
                    }
                }

                using (StreamWriter swr = new StreamWriter(outputPath))
                using (JsonWriter writer = new JsonTextWriter(swr))
                {
                    jsonSerializer.Serialize(writer, concordance);
                    //foreach (var word in concordance)
                    //{
                    //    swr.WriteLine(word.ToString());
                    //}
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
