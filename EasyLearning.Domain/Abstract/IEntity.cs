
namespace EasyLearning.Domain.Abstract
{
    public interface IEntity<T>
    {
        T ID { get; set; }
    }
}
