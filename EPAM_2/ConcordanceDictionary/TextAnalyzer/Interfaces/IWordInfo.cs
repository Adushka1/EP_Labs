using System.Collections.Generic;

namespace ConcordanceDictionary.TextAnalyzer.Interfaces
{
    public interface IWordInfo
    {
        string Word { get; }
        int WordAmount { get; }
        IList<int> LineNumbers { get; }

        void AddInfo(string word, int linePosition);
    }
}
