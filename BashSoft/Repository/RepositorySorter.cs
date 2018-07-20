namespace BashSoft.Repository
{
    using Interfaces;
    using IO;
    using Static_data;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RepositorySorter : IDataSorter
    {
        public void OrderAndTake(Dictionary<string, double> studentsWithMarks, string comparison,
            int studentsToTake)
        {
            comparison = comparison.ToLower();

            if (comparison == "ascending")
            {
                this.PrintStudents(studentsWithMarks
                    .OrderBy(x => x.Value)
                    .Take(studentsToTake)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            }
            else if (comparison == "descending")
            {
                this.PrintStudents(studentsWithMarks
                    .OrderByDescending(x => x.Value)
                    .Take(studentsToTake)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            }
            else
            {
                throw new ArgumentException(ExceptionMessage.InvalidComparisonQuery);
            }
        }

        private void PrintStudents(Dictionary<string, double> sortedStudents)
        {
            foreach (var kvp in sortedStudents)
            {
                OutputWriter.PrintStudent(kvp);
            }
        }
    }
}