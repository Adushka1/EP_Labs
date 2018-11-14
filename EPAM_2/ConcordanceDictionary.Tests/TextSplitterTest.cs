using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConcordanceDictionary.TextComponents;
using ConcordanceDictionary.TextComponents.Interfaces;
using Xunit;

namespace ConcordanceDictionary.Tests
{
    public class TextSplitterTest
    {
        private readonly ITextSplitter _textSplitter = new TextSplitter();

        public TextSplitterTest()
        {
        }

        [Fact]
        public void SplitTextWithSeparators()
        {
            //Arrange
            var text = "The World    ---?.,\n? Star??Platinum";
            var expected = new[] {"The", "World", "Star", "Platinum"};

            //Act
             var actual =_textSplitter.SplitLine(text);

            //Assert
            Assert.Equal(actual,expected);
        }

        [Fact]
        public void SplitEmptyString()
        {
            //Arrange
            var text = "           ";
            var expected ="";

            //Act
            var actual = _textSplitter.SplitLine(text).First();

            //Assert
            Assert.Equal(actual, expected);
        }

        
    }
}
