using System.Collections.Generic;
using System.Linq;
using ConcordanceDictionary.TextAnalyzer.Interfaces;

namespace ConcordanceDictionary.TextAnalyzer
{
    public class WordInfo : IWordInfo
    {
        public string Word { get; }
        public int WordAmount { get; private set; }
        public IList<int> LineNumbers { get; }

        public WordInfo(string word, int firstLineNumber)
        {
            Word = word;
            LineNumbers = new List<int> { firstLineNumber };
            WordAmount = 1;
        }

        public WordInfo(string word, IList<int> lineNumbers, int wordAmount)
        {
            Word = word;
            WordAmount = wordAmount;
            LineNumbers = lineNumbers;
        }

        public void AddInfo(string word, int linePosition)
        {
            WordAmount++;
            if (!LineNumbers.Contains(linePosition))
            {
                LineNumbers.Add(linePosition);
            }
        }

        public override string ToString()
        {
            return $"{Word} {WordAmount} {string.Join(",",LineNumbers.ToArray())}";
        }
    }
}

