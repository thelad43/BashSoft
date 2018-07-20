namespace BashSoft.IO.Commands
{
    using BashSoft.Attributes;
    using BashSoft.Interfaces;
    using Exceptions;

    [Alias("cdAbs")]
    public class ChangePathAbsoluteCommand : Command
    {
        [Inject]
        private readonly IDirectoryManager inputOutputManager;

        public ChangePathAbsoluteCommand(string input, string[] data)
            : base(input, data)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 2)
            {
                var absolutePath = this.Data[1];
                this.inputOutputManager.ChangeCurrentDirectoryAbsolute(absolutePath);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}