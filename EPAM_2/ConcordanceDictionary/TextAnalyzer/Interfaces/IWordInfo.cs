using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcordanceDictionary.TextAnalyzer.Interfaces
{
    public interface IWordInfo
    {
        string Word { get; }
        int WordAmount { get; set; }
        IList<int> LineNumbers { get; set; }

        string ToString();
    }
}
