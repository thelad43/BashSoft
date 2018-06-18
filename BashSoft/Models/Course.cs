namespace BashSoft.Models
{
    using Exceptions;
    using System.Collections.Generic;

    internal class Course
    {
        private string name;
        private readonly Dictionary<string, Student> studentsByName;

        internal const int NumberOfTasksOnExam = 5;
        internal const int MaxScoreOnExamTask = 100;

        public Course(string name)
        {
            this.name = name;
            this.studentsByName = new Dictionary<string, Student>();
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidStringException();
                }

                this.name = value;
            }
        }

        public IReadOnlyDictionary<string, Student> StudentsByName
        {
            get
            {
                return this.studentsByName;
            }
        }

        public void EnrollStudent(Student student)
        {
            if (this.studentsByName.ContainsKey(student.UserName))
            {
                throw new DuplicateEntryInStructureException(student.UserName, this.name);
            }

            this.studentsByName.Add(student.UserName, student);
        }
    }
}