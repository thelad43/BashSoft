﻿namespace BashSoft.IO.Commands
{
    using Exceptions;
    using Judge;
    using Repository;

    public class MakeDirectoryCommand : Command
    {
        public MakeDirectoryCommand(string input, string[] data, Tester judge, StudentsRepository repository, IOManager inputOutputManager)
            : base(input, data, judge, repository, inputOutputManager)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 2)
            {
                var folder = this.Data[1];
                this.InputOutputManager.CreateDirectoryInCurrentFolder(folder);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}