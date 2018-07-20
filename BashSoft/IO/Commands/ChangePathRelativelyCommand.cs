namespace BashSoft.IO.Commands
{
    using BashSoft.Attributes;
    using BashSoft.Interfaces;
    using Exceptions;

    [Alias("cdRel")]
    public class ChangePathRelativelyCommand : Command
    {
        [Inject]
        private readonly IDirectoryManager inputOutputManager;

        public ChangePathRelativelyCommand(string input, string[] data)
            : base(input, data)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 2)
            {
                var relPath = this.Data[1];
                this.inputOutputManager.ChangeCurrentDirectoryRelative(relPath);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}