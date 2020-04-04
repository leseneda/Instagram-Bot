using UpSocial.UpGram.Domain.Entity;

namespace UpSocial.UpGram.Domain.Interface
{
    public interface IRepository<T> : IRepositoryReadOnly<T> where T : BaseEntity
    {
        long Insert(T entity);

        bool Update(T entity);

        bool Remove(int id);

        bool Remove();
    }
}
