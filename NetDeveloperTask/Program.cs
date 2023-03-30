// See https://aka.ms/new-console-template for more information
using AppData;
using AppData.Interfaces;
using CommandLine;
using NetDeveloperTask.Commands;
using NetDeveloperTask.Interfaces;
using NetDeveloperTask.Validators;

namespace NetDeveloperTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var repository = new Repository();

            var validator = new ResourceShortageValidator();

            var commandService = new CommandService(repository, validator);

            Console.WriteLine("Welcome to Resource shortage management app. \nType help to list available commands");

            while (true)
            {
                var command = Console.ReadLine().Split(' ');

                Parser.Default.ParseArguments<RegisterShortageCommand, DeleteRequestCommand, ListRequestsCommand>(command)
                .WithParsed<ICommand>(t => t.Execute(commandService));
            }
        }
    }
}