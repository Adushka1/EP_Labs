using System;
using System.Configuration;
using System.IO;
using ConcordanceDictionary.TextAnalyzer;
using ConcordanceDictionary.TextAnalyzer.Interfaces;
using ConcordanceDictionary.TextWriter;
using ConcordanceDictionary.TextWriter.Interfaces;
using Newtonsoft.Json;

namespace ConcordanceDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            var readPath = ConfigurationManager.AppSettings["ReadPath"];
            var outputPath = ConfigurationManager.AppSettings["OutputPath"];
            var serializePath = ConfigurationManager.AppSettings["SerializePath"];
            var concordance = new Concordance();
            var subjectHeading = new SubjectHeading();

            try
            {
                using (var reader = new StreamReader(readPath))
                    concordance.ConcordsFill(reader);
                concordance.SortByKey();

                using (var swr = new StreamWriter(outputPath))
                    foreach (var line in subjectHeading.GetConcordanceLines(concordance))
                        swr.WriteLineAsync(line);

                using (var serializer = new StreamWriter(serializePath))
                    serializer.Write(JsonConvert.SerializeObject(concordance, Formatting.Indented));

            }
            catch (IOException e)
            {
                Console.WriteLine("Something went wrong with file");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
