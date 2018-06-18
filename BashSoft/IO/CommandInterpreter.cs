namespace BashSoft.IO
{
    using Exceptions;
    using IO.Commands;
    using Judge;
    using Repository;
    using System;
    using System.IO;

    internal class CommandInterpreter
    {
        private readonly Tester judge;
        private readonly StudentsRepository repository;
        private readonly IOManager inputOutputManager;

        public CommandInterpreter(Tester judge, StudentsRepository repository, IOManager inputOutputManager)
        {
            this.judge = judge;
            this.repository = repository;
            this.inputOutputManager = inputOutputManager;
        }

        public void InterpretCommand(string input)
        {
            var data = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var commandName = data[0].ToLower();

            try
            {
                var command = this.ParseCommand(input, data, commandName);
                command.Execute();
            }
            catch (DirectoryNotFoundException dnfe)
            {
                OutputWriter.DisplayException(dnfe.Message);
            }
            catch (ArgumentOutOfRangeException aoore)
            {
                OutputWriter.DisplayException(aoore.Message);
            }
            catch (ArgumentException ae)
            {
                OutputWriter.DisplayException(ae.Message);
            }
            catch (Exception e)
            {
                OutputWriter.DisplayException(e.Message);
            }
        }

        private Command ParseCommand(string input, string[] data, string command)
        {
            switch (command)
            {
                case "open":
                    return new OpenFileCommand(input, data, this.judge, this.repository, this.inputOutputManager);

                case "mkdir":
                    return new MakeDirectoryCommand(input, data, this.judge, this.repository, this.inputOutputManager);

                case "ls":
                    return new TraverseFoldersCommand(input, data, this.judge, this.repository, this.inputOutputManager);

                case "cmp":
                    return new CompareFilesCommand(input, data, this.judge, this.repository, this.inputOutputManager);

                case "cdRel":
                    return new ChangePathRelativelyCommand(input, data, this.judge, this.repository, this.inputOutputManager);

                case "cdAbs":
                    return new ChangePathAbsoluteCommand(input, data, this.judge, this.repository, this.inputOutputManager);

                case "readDb":
                    return new ReadDatabaseCommand(input, data, this.judge, this.repository, this.inputOutputManager);

                case "help":
                    return new GetHelpCommand(input, data, this.judge, this.repository, this.inputOutputManager);

                case "filter":
                    return new PrintFilteredStudentsCommand(input, data, this.judge, this.repository, this.inputOutputManager);

                case "order"://decOrder
                    return new PrintOrderedStudentsCommand(input, data, this.judge, this.repository, this.inputOutputManager);

                //case "decOrder":
                //    // TODO
                //    break;

                //case "download":
                //    // TODO
                //    break;

                case "downloadAsync":
                    return new GetHelpCommand(input, data, this.judge, this.repository, this.inputOutputManager);

                case "show":
                    return new ShowCourseCommand(input, data, this.judge, this.repository, this.inputOutputManager);

                case "dropdb":
                    return new DropDatabaseCommand(input, data, this.judge, this.repository, this.inputOutputManager);

                default:
                    throw new InvalidCommandException(input);
            }
        }
    }
}