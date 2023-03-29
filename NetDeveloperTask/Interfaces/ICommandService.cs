using AppData.Enums;
using AppData.Models;
using FluentValidation.Results;

namespace NetDeveloperTask.Interfaces
{
    public interface ICommandService
    {
        public ValidationResult AddResourceShortage(ResourceShortage resourceShortage);

        public List<ResourceShortage> GetResourceShortage(ResourceShortageFilter resourceShortageFilter);

        public bool RemoveResourceShortage(string title, Room room);
    }
}
