using System.Collections.Generic;
using System.Threading.Tasks;
using MeConecta.Gram.Domain.Entity;

namespace MeConecta.Gram.Domain.Interface
{
    public interface IRepositoryReadOnly<T> where T : BaseEntity
    {
        Task<T> SelectAsync(int id);

        Task<IEnumerable<T>> SelectAllAsync();
    }
}
