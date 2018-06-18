namespace BashSoft.IO.Commands
{
    using Exceptions;
    using Judge;
    using Repository;
    using Static_data;

    public class TraverseFoldersCommand : Command
    {
        public TraverseFoldersCommand(string input, string[] data, Tester judge, StudentsRepository repository, IOManager inputOutputManager)
            : base(input, data, judge, repository, inputOutputManager)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 1)
            {
                this.InputOutputManager.TraverseDirectory(0);
            }
            else if (this.Data.Length == 2)
            {
                var isParsed = int.TryParse(this.Data[1], out var depth);

                if (isParsed)
                {
                    this.InputOutputManager.TraverseDirectory(depth);
                }
                else
                {
                    OutputWriter.DisplayException(ExceptionMessage.UnableToParseNumber);
                }
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}