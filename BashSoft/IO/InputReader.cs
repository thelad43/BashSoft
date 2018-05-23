namespace BashSoft.IO
{
    using System;
    using BashSoft.Static_data;

    internal static class InputReader
    {
        private const string endCommand = "quit";

        public static void StartReadingCommands()
        {
            while (true)
            {
                OutputWriter.WriteMessage($"{SessionData.CurrentPath}>");
                var inputLine = Console.ReadLine().Trim();

                if (inputLine == endCommand)
                {
                    break;
                }

                CommandInterpreter.InterpredCommand(inputLine);
            }
        }
    }
}