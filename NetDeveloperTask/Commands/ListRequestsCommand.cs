using AppData.Enums;
using AppData.Models;
using CommandLine;
using NetDeveloperTask.Interfaces;
using System.Globalization;

namespace NetDeveloperTask.Commands
{
    [Verb("list", HelpText = "List requests with filters.")]
    public class ListRequestsCommand : ICommand
    {
        [Option('t', "title", Required = false, HelpText = "Filter with the title of the shortage registration.")]
        public string Title { get; set; }

        [Option("d1", Required = false, HelpText = "Date one. Specify d1 to find resource at particular date " +
            "or specify two dates to find resources in the interval of those days. Format: yyyy-mm-dd")]
        public string Date1 { get; set; }

        [Option("d2", Required = false, HelpText = "Date two. Specify d2 to find resource at particular date " +
            "or specify two dates to find resources in the interval of those days. Format: yyyy-mm-dd")]
        public string Date2 { get; set; }

        [Option('r', "Room", Required = false, HelpText = "Filter with the room where the shortage item is needed. Possible values: \n " +
            "1 - Meeting Room, 2 - Kitchen, 3 - Bathroom.")]
        public Room Room { get; set; }

        [Option('c', "Category", Required = false, HelpText = "Filter with the category of the shortage item. Possible values: \n " +
            "1 - Electronics, 2 - Food, 3 - Other.")]
        public Category Category { get; set; }

        public void Execute(ICommandService commandService)
        {
            DateTime temp;

            if (Date1 != null && !DateTime.TryParseExact(Date1, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
            {
                Console.WriteLine("Date one was written incorrectly");
                return;
            }

            if (Date2 != null && !DateTime.TryParseExact(Date2, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
            {
                Console.WriteLine("Date two was written incorrectly");
                return;
            }

            var result = commandService.GetResourceShortage(new ResourceShortageFilter()
            {
                Title = Title,
                Room = Room,
                Category = Category,
                Date1 = Date1,
                Date2 = Date2
            });

            result.ForEach(x => Console.WriteLine(x + "\n"));

            if(result.Count == 0)
            {
                Console.WriteLine("No resources have been found");
            }
        }
    }
}
