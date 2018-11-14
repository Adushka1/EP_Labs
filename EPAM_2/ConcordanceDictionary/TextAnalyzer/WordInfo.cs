using System.Collections.Generic;
using System.Linq;
using ConcordanceDictionary.TextAnalyzer.Interfaces;

namespace ConcordanceDictionary.TextAnalyzer
{
    public class WordInfo : IWordInfo

    {
    public string Word { get; }
    public int WordAmount { get; set; }
    public IList<int> LineNumbers { get; set; }

    public WordInfo(string word, int firstLineNumber)
    {
        Word = word;
        LineNumbers = new List<int> {firstLineNumber};
        WordAmount = 1;
    }

    public override string ToString()
    {
        var linePosition = LineNumbers.Aggregate("", (current, line) => current + line.ToString() + " ");

        return $"{Word}.................... {WordAmount}: {linePosition} ";
    }
    }
}
