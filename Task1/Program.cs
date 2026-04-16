using System.Text;

namespace Task1
{
    public class Program
    {
        private static void Main(string[] args)
        {
            string inputLine = "aaaaaaaaaaaab";
            string inputLine2 = "a11b2c13de";
            string outputLine = Compress(inputLine);
            string? outputLine2 = Decompress(inputLine2);
            Console.WriteLine("Input line: " + inputLine);
            Console.WriteLine("Compressed: " + outputLine);
            Console.WriteLine("Uncompressed: " + Decompress(outputLine));
            Console.WriteLine("Uncompressed2: " + Decompress(outputLine2));

            if (outputLine != null)
                Console.WriteLine(inputLine == Decompress(outputLine));
            else
                Console.WriteLine("Error");
        }

        public static string Compress(string? inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return String.Empty;

            StringBuilder resultString = new();
            char actualSymb = inputString[0];

            if (!IsItSmallLatinLetter(actualSymb))
                throw new FormatException("Invalid first character.");

            int actualSymbCounter = 1;

            for (int i = 1; i < inputString.Length; i++)
            {
                char tmpSymb = inputString[i];
                if (!IsItSmallLatinLetter(tmpSymb))
                    throw new FormatException($"Incorrect input format.{tmpSymb} is wrong character.");

                if (tmpSymb == actualSymb)
                {
                    actualSymbCounter++;
                }
                else
                {
                    AddNewPartCompress(resultString, actualSymb, actualSymbCounter);
                    actualSymb = tmpSymb;
                    actualSymbCounter = 1;
                }
            }

            AddNewPartCompress(resultString, actualSymb, actualSymbCounter);

            return resultString.ToString();
        }

        public static string Decompress(string? inputString)
        {
            StringBuilder resString = new StringBuilder();

            if (string.IsNullOrEmpty(inputString))
                return String.Empty;

            if (inputString.Length == 1 && IsItSmallLatinLetter(inputString[0]))
                return inputString;

            for (int i = 0; i < inputString.Length; i++)
            {
                char tmpSymb = inputString[i];
                if (!IsItSmallLatinLetter(tmpSymb))
                    throw new FormatException("Incorrect input format");

                int actualSymbCounter = 0;

                while (i < (inputString.Length - 1) && char.IsDigit(inputString[i + 1]))
                {
                    actualSymbCounter = actualSymbCounter * 10 + (inputString[i + 1] - '0');
                    i++;
                }
                resString.Append(tmpSymb, actualSymbCounter == 0 ? 1 : actualSymbCounter);
            }

            return resString.ToString();
        }


        private static void AddNewPartCompress(StringBuilder builder, char symb, int counter)
        {
            builder.Append(symb);
            if (counter > 1)
                builder.Append(counter);
        }


        private static bool IsItSmallLatinLetter(char c)
        {
            int intChar = (int)c;
            return (intChar >= 97 && intChar <= 122);
        }
    }
}
