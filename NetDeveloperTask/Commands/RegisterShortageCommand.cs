using AppData.Enums;
using AppData.Models;
using CommandLine;
using NetDeveloperTask.Interfaces;

namespace NetDeveloperTask.Commands
{
    [Verb("register", HelpText = "Register a new shortage.")]
    public class RegisterShortageCommand : ICommand
    {
        [Option('t', "title", Required = true, HelpText = "Title of the shortage registration.")]
        public string Title { get; set; }

        [Option('n', "name", Required = true, HelpText = "Name of the shortage item.")]
        public string Name { get; set; }

        [Option('r', "Room", Required = true, HelpText = "Room where the shortage item is needed. Possible values: \n " +
            "1 - Meeting Room, 2 - Kitchen, 3 - Bathroom.")]
        public Room Room { get; set; }

        [Option('c', "Category", Required = true, HelpText = "Category of the shortage item. Possible values: \n " +
             "1 - Electronics, 2 - Food, 3 - Other.")]
        public Category Category { get; set; }

        [Option('p', "Priority", Required = true, HelpText = "Priority of the resource request. Number 1-10, 1 - not important, 10 - very important.")]
        public int Priority { get; set; }


        public void Execute(ICommandService commandService)
        {
            var result = commandService.AddResourceShortage(new ResourceShortage()
            {
                Title = Title,
                Name = Name,
                Room = Room,
                Category = Category,
                Priority = Priority,
                CreatedOn = DateTime.Now.ToString("yyyy-MM-dd")
            });

            if (!result.IsValid)
            {
                //Only gets executed for priority because Room and Category invalid inputs
                //get handled by CommandLine.
                result.Errors.ForEach(error => { Console.WriteLine($"Property {error.PropertyName} is invalid: {error.ErrorMessage}"); });
                return;
            }
        }
    }
}
