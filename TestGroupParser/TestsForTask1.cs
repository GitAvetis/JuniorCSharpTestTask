using Task1;

namespace Tests
{
    public class TestsForTask1
    {
        [Fact]
        public void Compress_EmptyInput_ReturnsEmptyString()
        {
            string input = "";
            string output = Program.Compress(input);
            Assert.True(string.IsNullOrEmpty(output));
        }

        [Fact]
        public void Compress_ExampleFromTask_ReturnsCorrectRle()
        {
            string inputLine = "aaabbcccdde";
            string trueResult = "a3b2c3d2e";
            string output = Program.Compress(inputLine);
            Assert.Equal(trueResult, output);
        }

        [Fact]
        public void Compress_NoRepeatedCharacters_ReturnsSameString()
        {
            string inputLine = "abcde";
            string trueResult = "abcde";
            string output = Program.Compress(inputLine);
            Assert.Equal(trueResult, output);
        }

        [Fact]
        public void Compress_NullInput_ReturnsEmptyString()
        {
            string? inputLine = null;
            string output = Program.Compress(inputLine);
            Assert.True(string.IsNullOrEmpty(output));
        }

        [Fact]
        public void Compress_InvalidFirstCharacter_ThrowsFormatException()
        {
            string? inputLine = "1abcde";
            Exception ex = Assert.Throws<FormatException>(() => Program.Compress(inputLine));
            Assert.Contains("Invalid first character.", ex.Message);
        }

        [Fact]
        public void Decompress_InvalidSymbols_ThrowsFormatException()
        {
            string? inputLine = "a!b>c43de";
            Exception ex = Assert.Throws<FormatException>(() => Program.Decompress(inputLine));
            Assert.Contains("Incorrect input format", ex.Message);
        }

        [Fact]
        public void Decompress_MultiDigitCounts_ReturnsCorrectString()
        {
            string? inputLine = "a11b2c13de";
            string trueResult = "aaaaaaaaaaabbcccccccccccccde";
            string output = Program.Decompress(inputLine);
            Assert.Equal(trueResult, output);
        }

        [Fact]
        public void CompressDecompress_RoundTrip_ReturnsOriginalCompressedForm()
        {
            string? inputLine = "a11b2c13de";
            string decompressed = "aaaaaaaaaaabbcccccccccccccde";
            string output = Program.Compress(decompressed);
            Assert.Equal(inputLine, output);
        }

        [Fact]
        public void Decompress_EmptyInput_ReturnsEmptyString()
        {
            Assert.Equal(string.Empty, Program.Decompress(""));
        }

        [Fact]
        public void Decompress_SingleCharacter_ReturnsSameString()
        {
            Assert.Equal("a", Program.Decompress("a"));
        }

        [Fact]
        public void Decompress_NullInput_ReturnsEmptyString()
        {
            Assert.Equal(string.Empty, Program.Decompress(null));
        }
    }
}