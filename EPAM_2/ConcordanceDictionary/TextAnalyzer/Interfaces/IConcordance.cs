using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcordanceDictionary.TextComponents.Interfaces;

namespace ConcordanceDictionary.TextAnalyzer.Interfaces
{
    public interface IConcordance : IEnumerable
    {
        Dictionary<string, IWordInfo> WordInfos { get; }
        void GetWordInfos(string line, int linePosition);
    }
}
