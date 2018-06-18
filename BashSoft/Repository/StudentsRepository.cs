namespace BashSoft.Repository
{
    using Exceptions;
    using IO;
    using Models;
    using Static_data;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class StudentsRepository
    {
        private bool isDataInitialized;
        private Dictionary<string, Dictionary<string, List<int>>> studentsByCourse;
        private readonly RepositoryFilter filter;
        private readonly RepositorySorter sorter;
        private Dictionary<string, Course> courses;
        private Dictionary<string, Student> students;

        public StudentsRepository(RepositoryFilter filter, RepositorySorter sorter)
        {
            this.filter = filter;
            this.sorter = sorter;
            this.studentsByCourse = new Dictionary<string, Dictionary<string, List<int>>>();
        }

        public void LoadData(string fileName)
        {
            if (this.isDataInitialized)
            {
                OutputWriter.DisplayException(ExceptionMessage.DataAlreadyInitializedException);
                return;
            }

            this.students = new Dictionary<string, Student>();
            this.courses = new Dictionary<string, Course>();
            this.ReadData(fileName);
        }

        public void UnloadData()
        {
            if (!this.isDataInitialized)
            {
                throw new ArgumentException(ExceptionMessage.DataNotInitializedExceptionMessage);
            }

            this.students = null;
            this.courses = null;
            this.isDataInitialized = false;
        }

        private void ReadData(string fileName)
        {
            var path = SessionData.CurrentPath + "\\" + fileName;

            if (File.Exists(path))
            {
                const string pattern = @"([A-Z][a-zA-Z#\+]*_[A-Z][a-z]{2}_\d{4})\s+([A-Za-z]+\d{2}_\d{2,4})\s([\s0-9]+)";
                var regex = new Regex(pattern);
                var allInputLines = File.ReadAllLines(path);

                for (int line = 0; line < allInputLines.Length; line++)
                {
                    if (!string.IsNullOrEmpty(allInputLines[line]) && regex.IsMatch(allInputLines[line]))
                    {
                        var currentMatch = regex.Match(allInputLines[line]);
                        var courseName = currentMatch.Groups[1].Value;
                        var username = currentMatch.Groups[2].Value;
                        var scoresString = currentMatch.Groups[3].Value;

                        try
                        {
                            var scores = scoresString
                                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse)
                                .ToArray();

                            if (scores.Any(x => x > 100 || x < 0))
                            {
                                throw new ArgumentException(ExceptionMessage.InvalidScore);
                            }

                            if (scores.Length > Course.NumberOfTasksOnExam)
                            {
                                OutputWriter.DisplayException(ExceptionMessage.InvalidNumberOfScores);
                                continue;
                            }

                            if (!this.students.ContainsKey(username))
                            {
                                this.students.Add(username, new Student(username));
                            }

                            if (!this.courses.ContainsKey(courseName))
                            {
                                this.courses.Add(courseName, new Course(courseName));
                            }

                            var course = this.courses[courseName];
                            var student = this.students[username];

                            student.EnrollInCourse(course);
                            student.SetMarksInCourse(courseName, scores);

                            course.EnrollStudent(student);
                        }
                        catch (FormatException fex)
                        {
                            throw new FormatException(fex.Message + $"at line: {line}");
                        }
                    }
                }
            }
            else
            {
                throw new InvalidPathException();
            }

            this.isDataInitialized = true;
            OutputWriter.WriteMessageOnNewLine("Data read!");
        }

        private bool IsQueryForCoursePossible(string courseName)
        {
            if (this.courses.ContainsKey(courseName))
            {
                return true;
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessage.DataNotInitializedExceptionMessage);
            }

            if (this.studentsByCourse.ContainsKey(courseName))
            {
                return true;
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessage.InexistingCourseInDataBase);
            }

            return false;
        }

        private bool IsQueryForStudentPossiblе(string courseName, string studentUserName)
        {
            if (this.IsQueryForCoursePossible(courseName) &&
                this.courses[courseName].StudentsByName.ContainsKey(studentUserName))
            {
                return true;
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessage.InexistingStudentInDataBase);
            }

            return false;
        }

        public void GetStudentsScoresFromCourse(string courseName, string userName)
        {
            if (IsQueryForStudentPossiblе(courseName, userName))
            {
                OutputWriter.PrintStudent(new KeyValuePair<string, double>(userName, this.courses[courseName].StudentsByName[userName].MarksByCourseName[courseName]));
            }
        }

        public void GetAllStudentsFromCourse(string courseName)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                OutputWriter.WriteMessageOnNewLine(courseName);

                foreach (var studentMarksEntry in this.courses[courseName].StudentsByName)
                {
                    this.GetStudentsScoresFromCourse(courseName, studentMarksEntry.Key);
                }
            }
        }

        public void FilterAndTake(string courseName, string givenFilter, int? studentsToTake = null)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = this.studentsByCourse[courseName].Count;
                }

                var marks = this.courses[courseName].StudentsByName.ToDictionary(x => x.Key, y => y.Value.MarksByCourseName[courseName]);
                this.filter.FilterAndTake(marks, givenFilter, studentsToTake.Value);
            }
        }

        public void OrderAndTake(string courseName, string comparison, int? studentsToTake = null)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = this.courses[courseName].StudentsByName.Count;
                }

                var marks = this.courses[courseName].StudentsByName.ToDictionary(x => x.Key, y => y.Value.MarksByCourseName[courseName]);
                this.sorter.OrderAndTake(marks, comparison, studentsToTake.Value);
            }
        }
    }
}