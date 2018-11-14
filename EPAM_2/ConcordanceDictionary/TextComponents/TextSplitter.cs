using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ConcordanceDictionary.TextComponents.Interfaces;

namespace ConcordanceDictionary.TextComponents
{
    public class TextSplitter : ITextSplitter
    {
        public string Pattern { get; }

        public TextSplitter()
        {
            Pattern = @"[^a-zA-Z]+";
        }

        public string[] SplitLine(string line)
        {
            return Regex.Split(line, Pattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        }
    }
}
