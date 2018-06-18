namespace BashSoft.Exceptions
{
    using System;

    public class DuplicateEntryInStructureException : Exception
    {
        private const string DuplicateEntry = "Student already enrolled in given course";

        public DuplicateEntryInStructureException(string message)
            : base(message)
        {
        }

        public DuplicateEntryInStructureException(string entry, string structure)
            : base(string.Format(DuplicateEntry, entry, structure))
        {
        }
    }
}