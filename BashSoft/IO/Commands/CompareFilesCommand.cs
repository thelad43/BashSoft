namespace BashSoft.IO.Commands
{
    using BashSoft.Attributes;
    using BashSoft.Interfaces;
    using Exceptions;

    [Alias("cmp")]
    public class CompareFilesCommand : Command
    {
        [Inject]
        private readonly IContentComparer judge;

        public CompareFilesCommand(string input, string[] data)
            : base(input, data)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 3)
            {
                var firstPath = this.Data[1];
                var secondPath = this.Data[2];
                this.judge.CompareContent(firstPath, secondPath);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}