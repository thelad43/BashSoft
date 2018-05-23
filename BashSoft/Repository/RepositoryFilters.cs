namespace BashSoft.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BashSoft.IO;
    using BashSoft.Static_data;

    internal static class RepositoryFilters
    {
        public static void FilterAndTake(Dictionary<string, List<int>> wantedData,
            string wantedFilter, int studentsToTake)
        {
            if (wantedFilter == "excellent")
            {
                FilterAndTake(wantedData, x => x >= 5, studentsToTake);
            }
            else if (wantedFilter == "average")
            {
                FilterAndTake(wantedData, x => x >= 3.5 && x < 5, studentsToTake);
            }
            else if (wantedFilter == "poor")
            {
                FilterAndTake(wantedData, x => x < 3.5, studentsToTake);
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessage.InvalidStudentFilter);
            }
        }

        private static void FilterAndTake(Dictionary<string, List<int>> wantedData,
            Predicate<double> givenFilter, int studentsToTake)
        {
            var countOfPrinted = 0;

            foreach (var studentPoints in wantedData)
            {
                if (countOfPrinted == studentsToTake)
                {
                    break;
                }

                var averageScore = studentPoints.Value.Average();
                var percentageOfFullfillment = averageScore / 100;
                var averageMark = percentageOfFullfillment * 4 + 2;

                if (givenFilter(averageMark))
                {
                    OutputWriter.PrintStudent(studentPoints);
                    countOfPrinted++;
                }
            }
        }
    }
}