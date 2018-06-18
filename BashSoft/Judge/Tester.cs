namespace BashSoft.Judge
{
    using IO;
    using Static_data;
    using System;
    using System.IO;

    public class Tester
    {
        public void CompareContent(string userOutputPath, string expectedOutputPath)
        {
            try
            {
                OutputWriter.WriteMessageOnNewLine("Reading files...");
                var mismatchPath = GetMismatchPath(expectedOutputPath);
                var actualOutputLines = File.ReadAllLines(userOutputPath);
                var expectedOutputLines = File.ReadAllLines(expectedOutputPath);

                var mismatches = GetLinesWithPossibleMismatches(actualOutputLines, expectedOutputPath, out var hasMismatch);

                this.PrintOutput(mismatches, hasMismatch, mismatchPath);

                OutputWriter.WriteMessageOnNewLine("Files Read!");
            }
            catch (IOException)
            {
                // OutputWriter.DisplayException(ExceptionMessage.InvalidPath);
            }
        }

        private void PrintOutput(string[] mismatches, bool hasMismatch, string mismatchPath)
        {
            if (hasMismatch)
            {
                foreach (var line in mismatches)
                {
                    OutputWriter.WriteMessageOnNewLine(line);
                }

                File.WriteAllLines(mismatchPath, mismatches);

                return;
            }

            OutputWriter.WriteMessageOnNewLine("Files are identical. There are no mismatches.");
        }

        private string[] GetLinesWithPossibleMismatches(string[] actualOutputLines, string expectedOutputPath, out bool hasMismatch)
        {
            hasMismatch = false;
            var output = string.Empty;

            var minOutputLines = actualOutputLines.Length;

            if (actualOutputLines.Length != expectedOutputPath.Length)
            {
                hasMismatch = true;
                minOutputLines = Math.Min(actualOutputLines.Length, expectedOutputPath.Length);
                OutputWriter.DisplayException(ExceptionMessage.ComparisonOfFilesWithDifferentSizes);
            }

            var mismatches = new string[actualOutputLines.Length];
            OutputWriter.WriteMessageOnNewLine("Comparing files...");

            for (int index = 0; index < minOutputLines; index++)
            {
                var actualLine = actualOutputLines[index];
                var expectedLine = expectedOutputPath[index];

                if (!actualLine.Equals(expectedLine))
                {
                    output = string.Format($"Mismatch at line {index} -- expected: \"{expectedLine}\", actual: \"{actualLine}\"");
                    output += Environment.NewLine;
                    hasMismatch = true;
                }
                else
                {
                    output = actualLine;
                    output += Environment.NewLine;
                }

                mismatches[index] = output;
            }

            return mismatches;
        }

        private string GetMismatchPath(string expectedOutputPath)
        {
            var indexOf = expectedOutputPath.LastIndexOf('\\');
            var directoryPath = expectedOutputPath.Substring(0, indexOf);
            var mismatchPath = directoryPath + @"\Mismatch.txt";
            return mismatchPath;
        }
    }
}