namespace BashSoft.IO
{
    using Interfaces;
    using Static_data;
    using System;

    public class InputReader : IReader
    {
        private const string endCommand = "quit";
        private readonly IInterpreter interpreter;

        public InputReader(IInterpreter interpreter)
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