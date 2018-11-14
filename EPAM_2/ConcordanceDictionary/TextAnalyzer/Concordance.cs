using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ConcordanceDictionary.TextAnalyzer.Interfaces;
using ConcordanceDictionary.TextComponents;
using ConcordanceDictionary.TextComponents.Interfaces;

namespace ConcordanceDictionary.TextAnalyzer
{
    public class Concordance : IConcordance
    {
        private readonly ITextSplitter _textSplitter;
        public Dictionary<string, IWordInfo> WordInfos { get; }



        public Concordance()
        {
            _textSplitter = new TextSplitter();
            WordInfos = new Dictionary<string, IWordInfo>();
        }

        public void GetWordInfos(string line, int linePosition)
        {
            foreach (var word in _textSplitter.SplitLine(line))
            {
                var lowerWord = word.ToLower();
                if (!WordInfos.ContainsKey(lowerWord))
                {
                    WordInfos.Add(lowerWord, new WordInfo(lowerWord, linePosition));
                }
                else
                {
                    WordInfos[lowerWord].WordAmount++;
                    if (!WordInfos[lowerWord].LineNumbers.Contains(linePosition))
                    {
                        WordInfos[lowerWord].LineNumbers.Add(linePosition);
                    }
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            return WordInfos.GetEnumerator();
        }
    }
}
