using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConcordanceDictionary.TextAnalyzer;
using ConcordanceDictionary.TextAnalyzer.Interfaces;
using ConcordanceDictionary.TextWriter.Interfaces;

namespace ConcordanceDictionary.TextWriter
{
    public class SubjectHeading : ISubjectHeading
    {
        public IEnumerable<string> GetConcordanceLines(IConcordance concordance)
        {
            var previousWordFirstSymbol = '\0';

            foreach (var concord in concordance.Concords)
            {
                if (concord.Key[0] != previousWordFirstSymbol)
                {
                    previousWordFirstSymbol = concord.Key[0];

                    yield return GetLeadingSymbol(concord.Key);
                    yield return GetConcordanceLine(concord);

                    continue;
                }

                yield return GetConcordanceLine(concord);
            }
        }

        private static string GetLeadingSymbol(string concordKey)
        {
            return concordKey.FirstOrDefault().ToString().ToUpper();
        }

        private static string GetConcordanceLine(KeyValuePair<string, WordInfo> concord)
        {
            var stringBuilder = new StringBuilder();
            var linePosition = concord.Value.LineNumbers.Aggregate("", (current, line) => current + line.ToString() + " ");
            return stringBuilder.AppendFormat("{0, -15} {1, -3} : {2, -2}",
                 concord.Key, concord.Value.WordAmount, linePosition).ToString();
        }
    }
}
