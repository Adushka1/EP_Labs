using System.Collections.Generic;
using System.IO;
using System.Text;
using ConcordanceDictionary.TextAnalyzer;
using ConcordanceDictionary.TextAnalyzer.Interfaces;
using Xunit;

namespace ConcordanceDictionary.Tests
{
    public class ConcordanceTest
    {
        private IConcordance concordance = new Concordance();

        [Fact]
        public void ConcordText()
        {
            //Arrange
            var input = "Hello\n World Hello";
            var unicodeEncoding = new UnicodeEncoding();
            var ms = new MemoryStream(unicodeEncoding.GetBytes(input));
            var expected = new Dictionary<string, IWordInfo>()
            {
                ["hello"] = new WordInfo(word: "hello", lineNumbers: new List<int>() { 1, 2 }, wordAmount: 2),
                ["world"] = new WordInfo(word: "world", lineNumbers: new List<int>() { 1 }, wordAmount: 1)
            };

            //Act
            using (var sr = new StreamReader(ms, System.Text.Encoding.Unicode))
                concordance.ConcordsFill(sr);
            var actual = concordance.Concords;

            //Assert
            Assert.Equal(actual.ToString(), expected.ToString());
        }
    }
}
