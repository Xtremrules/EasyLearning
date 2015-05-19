using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyLearning.Domain.Abstract.Service
{
    public interface IEntityService<T> : IService where T : BaseEntity
    {
        Task CreateAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
        IEnumerable<T> GetAll();
    }
}
