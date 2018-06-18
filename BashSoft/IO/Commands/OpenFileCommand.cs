namespace BashSoft.IO.Commands
{
    using Exceptions;
    using Judge;
    using Repository;
    using Static_data;
    using System.Diagnostics;

    public class OpenFileCommand : Command
    {
        public OpenFileCommand(string input, string[] data, Tester judge, StudentsRepository repository, IOManager inputOutputManager)
            : base(input, data, judge, repository, inputOutputManager)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 2)
            {
                var fileName = this.Data[1];
                Process.Start(SessionData.CurrentPath + "\\" + fileName);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}