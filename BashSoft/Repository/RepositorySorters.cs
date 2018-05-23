namespace BashSoft.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using BashSoft.IO;
    using BashSoft.Static_data;

    internal static class RepositorySorters
    {
        public static void OrderAndTake(Dictionary<string, List<int>> wantedData, string comparison,
            int studentsToTake)
        {
            comparison = comparison.ToLower();

            if (comparison == "ascending")
            {
                PrintStudents(wantedData
                    .OrderBy(x => x.Value.Sum())
                    .Take(studentsToTake)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            }
            else if (comparison == "descending")
            {
                PrintStudents(wantedData
                    .OrderByDescending(x => x.Value.Sum())
                    .Take(studentsToTake)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessage.InvalidComparisonQuery);
            }
        }

        private static void PrintStudents(Dictionary<string, List<int>> sortedStudents)
        {
            foreach (var kvp in sortedStudents)
            {
                OutputWriter.PrintStudent(kvp);
            }
        }
    }
}