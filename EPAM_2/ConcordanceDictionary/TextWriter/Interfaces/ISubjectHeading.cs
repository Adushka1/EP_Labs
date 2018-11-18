using System.Collections.Generic;
using ConcordanceDictionary.TextAnalyzer.Interfaces;

namespace ConcordanceDictionary.TextWriter.Interfaces
{
    public interface ISubjectHeading
    {
        IEnumerable<string> GetConcordanceLines(IConcordance concordance);
    }
}