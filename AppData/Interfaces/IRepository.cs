using AppData.Models;
using System.Linq.Expressions;

namespace AppData.Interfaces
{
    public interface IRepository
    {
        public IQueryable<ResourceShortage> FindAllOrderedPriority();

        public ResourceShortage FindFirstByCondition(Expression<Func<ResourceShortage, bool>> expression);

        public int IndexOfResourceShortage(ResourceShortage resourceShortage);

        public void AddResourceShortage(ResourceShortage resourceShortage);

        public void RemoveResourceShortage(ResourceShortage resourceShortage);

        public void ReplaceResourceShortageAt(int index, ResourceShortage resourceShortageReplacement);
    }
}
