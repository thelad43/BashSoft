namespace BashSoft.IO
{
    using BashSoft.Attributes;
    using Exceptions;
    using Interfaces;
    using IO.Commands;
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public class CommandInterpreter : IInterpreter
    {
        private readonly IContentComparer judge;
        private readonly IDatabase repository;
        private readonly IDirectoryManager inputOutputManager;

        public CommandInterpreter(IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager)
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

        private IExecutable ParseCommand(string input, string[] data, string command)
        {
            var parametersForConstruction = new object[]
            {
               input, data
            };

            var assembly = Assembly.GetExecutingAssembly();

            var typeOfCommand = assembly
                .GetTypes()
                .First(type => type.GetCustomAttributes(typeof(AliasAttribute))
                .Where(atr => atr.Equals(command))
                .ToArray()
                .Length > 0);

            if (typeOfCommand is null)
            {
                throw new InvalidCommandException(command);
            }

            var typeInterpreter = typeof(CommandInterpreter);
            var exe = (Command)Activator.CreateInstance(typeOfCommand, parametersForConstruction);

            var fieldsOfCommand = typeOfCommand.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            var fieldsOfInterpreter = typeInterpreter.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var fieldOfCommand in fieldsOfCommand)
            {
                var injectAttribute = fieldOfCommand.GetCustomAttribute(typeof(InjectAttribute));

                if (injectAttribute != null)
                {
                    if (fieldsOfInterpreter.Any(x => x.FieldType == fieldOfCommand.FieldType))
                    {
                        fieldOfCommand
                            .SetValue(exe, fieldsOfInterpreter.First(x => x.FieldType == fieldOfCommand.FieldType).GetValue(this));
                    }
                }
            }

            return exe;
        }
    }
}