namespace BashSoft.Repository
{
    using Interfaces;
    using IO;
    using Static_data;
    using System;
    using System.Collections.Generic;

    public class RepositoryFilter : IDataFilter
    {
        public void FilterAndTake(Dictionary<string, double> studentsWithMarks,
            string wantedFilter, int studentsToTake)
        {
            if (wantedFilter == "excellent")
            {
                this.FilterAndTake(studentsWithMarks, x => x >= 5, studentsToTake);
            }
            else if (wantedFilter == "average")
            {
                FilterAndTake(studentsWithMarks, x => x >= 3.5 && x < 5, studentsToTake);
            }
            else if (wantedFilter == "poor")
            {
                FilterAndTake(studentsWithMarks, x => x < 3.5, studentsToTake);
            }
            else
            {
                throw new ArgumentException(ExceptionMessage.InvalidStudentFilter);
            }
        }

        private void FilterAndTake(Dictionary<string, double> studentsWithMarks,
            Predicate<double> givenFilter, int studentsToTake)
        {
            var countOfPrinted = 0;

            foreach (var studentMarks in studentsWithMarks)
            {
                if (countOfPrinted == studentsToTake)
                {
                    break;
                }

                if (givenFilter(studentMarks.Value))
                {
                    OutputWriter.PrintStudent(new KeyValuePair<string, double>(studentMarks.Key, studentMarks.Value));
                    countOfPrinted++;
                }
            }
        }
    }
}