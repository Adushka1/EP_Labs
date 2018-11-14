using System.IO;

namespace ConcordanceDictionary.TextComponents.Interfaces
{
    public interface ITextSplitter
    {
        string Pattern { get; }
        string[] SplitLine(string line);
    }
}
