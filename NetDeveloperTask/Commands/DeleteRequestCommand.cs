using AppData.Enums;
using CommandLine;
using NetDeveloperTask.Interfaces;

namespace NetDeveloperTask.Commands
{
    [Verb("delete", HelpText = "Delete a request.")]
    public class DeleteRequestCommand : ICommand
    {
        [Option('t', "title", Required = true, HelpText = "Title of the shortage registration.")]
        public string Title { get; set; }

        [Option('r', "Room", Required = true, HelpText = "Room where the shortage item is needed. Possible values: \n " +
            "1 - Meeting Room, 2 - Kitchen, 3 - Bathroom.")]
        public Room Room { get; set; }

        public void Execute(ICommandService commandService)
        {
            if (commandService.RemoveResourceShortage(Title, Room))
            {
                Console.WriteLine("Resource has been deleted");
                return;
            }

            Console.WriteLine("Resource does not exist");
        }
    }
}
