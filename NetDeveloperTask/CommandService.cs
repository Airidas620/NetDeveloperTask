using AppData.Enums;
using AppData.Interfaces;
using AppData.Models;
using FluentValidation.Results;
using NetDeveloperTask.Interfaces;
using NetDeveloperTask.Validators;
using System.Security.Principal;

namespace NetDeveloperTask
{
    public class CommandService : ICommandService
    {
        private string _userName;

        private bool _isAdministrator;

        private IRepository _repository;

        private ResourceShortageValidator _validator;

        public CommandService(IRepository repository, ResourceShortageValidator validator)
        {
            _repository = repository;

            _validator = validator;

            _userName = Environment.UserName;

            //Only works for windows
            _isAdministrator = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }

        public ValidationResult AddResourceShortage(ResourceShortage resourceShortage)
        {
            resourceShortage.UserName = _userName;

            var result = _validator.Validate(resourceShortage);

            if (!result.IsValid)
            {
                return result;
            }

            //Check if the resource already exist
            var sameResource = _repository.FindFirstByCondition(
                r => r.Title.Equals(resourceShortage.Title) && r.Room.Equals(resourceShortage.Room));

            if (sameResource != null)
            {
                if (resourceShortage.Priority > sameResource.Priority)
                {
                    int resourceIndex = _repository.IndexOfResourceShortage(sameResource);

                    _repository.ReplaceResourceShortageAt(resourceIndex, resourceShortage);

                    return result;
                }

                Console.WriteLine("The resource already exist and it has greater or equal priority");

                return result;
            }

            //If it doesn't exist add it to the collection
            _repository.AddResourceShortage(resourceShortage);

            return result;
        }

        public bool RemoveResourceShortage(string title, Room room)
        {
            var resource = _repository.FindFirstByCondition(r => r.Title.Equals(title) && r.Room.Equals(room));

            if (resource == null)
            {
                return false;
            }

            if (_isAdministrator || resource.UserName == _userName)
            {
                _repository.RemoveResourceShortage(resource);
                return true;
            }

            return false;
        }
        public List<ResourceShortage> GetResourceShortage(ResourceShortageFilter filter)
        {
            var query = _repository.FindAllOrderedPriority();

            query = query.Where(r => (filter.Title != null) ? r.Title.ToLower().Contains(filter.Title.ToLower()) : true)
                              .Where(r => (filter.Room != 0) ? r.Room == filter.Room : true)
                              .Where(r => (filter.Category != 0) ? r.Category == filter.Category : true)
                              .Where(r => (filter.Date1 != null && filter.Date2 == null) ? r.CreatedOn == filter.Date1 : true)
                              .Where(r => (filter.Date1 != null && filter.Date2 != null) ?
                              stringToDateTime(r.CreatedOn) >= stringToDateTime(filter.Date1) && stringToDateTime(r.CreatedOn) <= stringToDateTime(filter.Date2) : true);

            if (!_isAdministrator)
            {
                query = query.Where(r => r.UserName == _userName);
            }

            return query.ToList();
        }

        private DateTime stringToDateTime(string date)
        {
            return DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
