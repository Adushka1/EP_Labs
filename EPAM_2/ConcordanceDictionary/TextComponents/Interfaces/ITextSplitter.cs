using System.IO;

namespace ConcordanceDictionary.TextComponents.Interfaces
{
    public interface ITextSplitter
    {
        string[] SplitLine(string line);
    }
}
