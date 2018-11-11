using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcordanceDictionary.TextAnalyzer;

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
                Concordance concordance = new Concordance();
                using (StreamReader sr = new StreamReader(readPath))
                {
                    concordance.GetWordInfos(sr);
                }

                using (StreamWriter swr = new StreamWriter(outputPath))
                {
                    foreach (var word in concordance)
                    {
                        swr.WriteLine(word.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
