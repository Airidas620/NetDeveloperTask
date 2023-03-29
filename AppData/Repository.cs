using AppData.Interfaces;
using AppData.Models;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace AppData
{
    public class Repository : IRepository
    {
        private const string _jsonFileName = (@"..\..\..\..\AppData\" + "ResourceShortageData.json");

        public const int NotInStructure = -1;

        public List<ResourceShortage> _shortageResources { get; private set; }

        public Repository()
        {
            string jsonData = File.ReadAllText(_jsonFileName);
            _shortageResources = JsonConvert.DeserializeObject<List<ResourceShortage>>(jsonData);

            if (_shortageResources == null)
            {
                _shortageResources = new List<ResourceShortage>();
            }
        }

        public void AddResourceShortage(ResourceShortage shortageResource)
        {
            if (shortageResource != null)
            {
                _shortageResources.Add(shortageResource);
                SaveDataToJson();
            }
        }

        public void RemoveResourceShortage(ResourceShortage resourceShortage)
        {
            if (resourceShortage == null)
            {
                return;
            }

            int resourceIndex = IndexOfResourceShortage(resourceShortage);

            if (resourceIndex != NotInStructure)
            {
                _shortageResources.RemoveAt(resourceIndex);
                SaveDataToJson();
            }
        }

        public int IndexOfResourceShortage(ResourceShortage resourceShortage)
        {
            if (resourceShortage == null)
            {
                return NotInStructure;
            }

            return _shortageResources.FindIndex(r => r.Equals(resourceShortage));
        }

        public void ReplaceResourceShortageAt(int index, ResourceShortage resourceShortageReplacement)
        {
            _shortageResources[index] = resourceShortageReplacement;
            SaveDataToJson();
        }

        public ResourceShortage FindFirstByCondition(Expression<Func<ResourceShortage, bool>> expression)
        {
            return _shortageResources.AsQueryable().FirstOrDefault(expression);
        }

        public IQueryable<ResourceShortage> FindAllOrderedPriority()
        {
            return _shortageResources.AsQueryable().OrderByDescending(r => r.Priority);
        }

        private void SaveDataToJson()
        {
            File.WriteAllText(_jsonFileName, JsonConvert.SerializeObject(_shortageResources));
        }
    }
}
