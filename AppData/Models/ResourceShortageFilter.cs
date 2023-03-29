using AppData.Enums;

namespace AppData.Models
{
    public class ResourceShortageFilter
    {
        public string Title { get; set; }

        public string Date1 { get; set; }

        public string Date2 { get; set; }

        public Room Room { get; set; }

        public Category Category { get; set; }
    }
}
