namespace BashSoft.IO
{
    using Static_data;
    using System;

    internal class InputReader
    {
        private const string endCommand = "quit";
        private readonly CommandInterpreter interpreter;

        public InputReader(CommandInterpreter interpreter)
        {
            this.interpreter = interpreter;
        }

        public void StartReadingCommands()
        {
            while (true)
            {
                OutputWriter.WriteMessage($"{SessionData.CurrentPath}>");
                var inputLine = Console.ReadLine().Trim();

                if (inputLine == endCommand)
                {
                    break;
                }

                this.interpreter.InterpretCommand(inputLine);
            }
        }
    }
}