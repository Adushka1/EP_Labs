using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ConcordanceDictionary.TextAnalyzer
{
    public class Concordance : IEnumerable
    {
        private const string Pattern = @"[^a-zA-Z]+";

        public Dictionary<string, WordInfo> WordInfos { get; }

        public Concordance()
        {
            WordInfos = new Dictionary<string, WordInfo>();
        }

        public void GetWordInfos(StreamReader sr)
        {
            var i = 0;
            while (sr.Peek() >= 0)
            {
                var line = sr.ReadLine();
                foreach (var word in SplitLine(line))
                {
                    if (!WordInfos.ContainsKey(word))
                    {
                        WordInfos.Add(word, new WordInfo(word, i));
                    }
                    else
                    {
                        WordInfos[word].WordAmount++;
                        if (!WordInfos[word].LineNumbers.Contains(i))
                        {
                            WordInfos[word].LineNumbers.Add(i);
                        }
                    }
                }
                i++;
            }
        }

        public string[] SplitLine(string line)
        {
            return Regex.Split(line, Pattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        }

        public IEnumerator GetEnumerator()
        {
            return WordInfos.GetEnumerator();
        }
    }
}
