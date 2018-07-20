namespace BashSoft.IO.Commands
{
    using BashSoft.Attributes;
    using BashSoft.Interfaces;
    using Exceptions;

    [Alias("mkdir")]
    public class MakeDirectoryCommand : Command
    {
        [Inject]
        private readonly IDirectoryManager inputOutputManager;

        public MakeDirectoryCommand(string input, string[] data)
            : base(input, data)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 2)
            {
                var folder = this.Data[1];
                this.inputOutputManager.CreateDirectoryInCurrentFolder(folder);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}