using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConcordanceDictionary.TextAnalyzer.Interfaces;
using ConcordanceDictionary.TextComponents;
using ConcordanceDictionary.TextComponents.Interfaces;

namespace ConcordanceDictionary.TextAnalyzer
{
    public class Concordance : IConcordance
    {
        private readonly ITextSplitter _textSplitter;
        public Dictionary<string, WordInfo> Concords { get; private set; }

        public Concordance()
        {
            _textSplitter = new TextSplitter();
            Concords = new Dictionary<string, WordInfo>();
        }

        public void ConcordsFill(StreamReader reader)
        {
            var linePosition = 1;

            while (reader.Peek() >= 0)
            {
                var line = reader.ReadLine();

                foreach (var word in _textSplitter.SplitLine(line))
                {
                    var lowerWord = word.ToLower();
                    if (!Concords.ContainsKey(lowerWord))
                    {
                        Concords.Add(lowerWord, new WordInfo(lowerWord, linePosition));
                    }
                    else
                    {
                        Concords[lowerWord].AddInfo(lowerWord, linePosition);
                    }
                }
                linePosition++;
            }
        }

        public void SortByKey()
        {
            Concords = Concords.OrderBy(x => x.Key).ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
        }

        public IEnumerator GetEnumerator()
        {
            return Concords.GetEnumerator();
        }
    }
}
