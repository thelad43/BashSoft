namespace BashSoft
{
    using IO;
    using Judge;
    using Repository;

    public class StartUp
    {
        public static void Main()
        {
            var tester = new Tester();
            var ioManager = new IOManager();
            var repository = new StudentsRepository(new RepositoryFilter(), new RepositorySorter());

            var currentInterpreter = new CommandInterpreter(tester, repository, ioManager);
            var reader = new InputReader(currentInterpreter);

            reader.StartReadingCommands();
        }
    }
}