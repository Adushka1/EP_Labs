using System;
using Xunit;
using  ConcordanceDictionary;
using ConcordanceDictionary.TextAnalyzer;

namespace ConcordanceDictionary.Tests
{
    public class ConcordanceTest
    {
        private readonly Concordance _concordance;

        public ConcordanceTest()
        {
            _concordance = new Concordance();
        }

        [Fact]
        public void SplitUsualText()
        {
            //Arrange
            var text = "The World Star Platinum";
            var expected = new[] { "The", "World", "Star", "Platinum" };

            //Act
            var actual = _concordance.SplitLine(text);
            
            //Assert
            Assert.Equal(expected,actual);
        }
        [Fact]
        public void SplitSentenceWithPunctuation()
        {
            //Arrange
            var text = "The, World --   Star? Platinum";
            var expected = new[] { "The", "World", "Star", "Platinum" };
          
            //Act
            var actual = _concordance.SplitLine(text);
            
            //Assert
            Assert.Equal(expected, actual);
        }

    }
}
