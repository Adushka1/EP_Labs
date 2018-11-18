using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ConcordanceDictionary.TextAnalyzer.Interfaces
{
    public interface IConcordance : IEnumerable
    {
        Dictionary<string, WordInfo> Concords { get; }
        void ConcordsFill(StreamReader reader);
        void SortByKey();
    }
}
