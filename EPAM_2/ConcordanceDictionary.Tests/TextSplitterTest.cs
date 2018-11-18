using System.Linq;
using ConcordanceDictionary.TextComponents;
using ConcordanceDictionary.TextComponents.Interfaces;
using Xunit;

namespace ConcordanceDictionary.Tests
{
    public class TextSplitterTest
    {

        [Theory]
        [InlineData("The World    ---?.,\n? Star??Platinum", new[] { "The", "World", "Star", "Platinum" })]
        [InlineData("Pluto -- Planet", new[] { "Pluto", "Planet" })]
        [InlineData("Slo1Mo", new[] { "Slo", "Mo" })]
        public void SplitTextWithSeparators(string input, string[] expected)
        {
            //Arrange
            ITextSplitter textSplitter = new TextSplitter();

            //Act
            var actual = textSplitter.SplitLine(input);

            //Assert
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void SplitEmptyString()
        {
            //Arrange
            ITextSplitter textSplitter = new TextSplitter();
            var text = "           ";
            var expected = "";

            //Act
            var actual = textSplitter.SplitLine(text).First();

            //Assert
            Assert.Equal(actual, expected);
        }
    }
}
