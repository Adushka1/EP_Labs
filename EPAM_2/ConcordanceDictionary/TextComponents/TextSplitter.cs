using System.Text.RegularExpressions;
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

        public TextSplitter(string pattern)
        {
            Pattern = pattern;
        }

        public string[] SplitLine(string line)
        {
            return Regex.Split(line, Pattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        }
    }
}
