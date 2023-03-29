using AppData.Enums;

namespace AppData.Models
{
    public class ResourceShortage
    {
        public string UserName { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public Room Room { get; set; }

        public Category Category { get; set; }

        public int Priority { get; set; }

        public string CreatedOn { get; set; }

        public override string ToString()
        {
            return $"Title: {Title} \nName: {Name} \nRoom: {Room} \nCategory: {Category} \nPriority: {Priority} \nCreated on: {CreatedOn}";
        }
    }
}
