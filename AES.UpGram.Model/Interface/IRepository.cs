using UpSocial.UpGram.Domain.Entity;

namespace UpSocial.UpGram.Domain.Interface
{
    public interface IRepository<T> : IRepositoryReadOnly<T> where T : BaseEntity
    {
        void Insert(T entity);

        void Update(T entity);

        void Remove(int id);

    }
}
