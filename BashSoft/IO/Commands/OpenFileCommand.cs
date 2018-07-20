namespace BashSoft.IO.Commands
{
    using BashSoft.Attributes;
    using Exceptions;
    using Static_data;
    using System.Diagnostics;

    [Alias("open")]
    public class OpenFileCommand : Command
    {
        public OpenFileCommand(string input, string[] data)
            : base(input, data)
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