using System.Collections.Generic;
using System.Threading.Tasks;

namespace UpSocial.UpGram.Domain.Interface
{
    public interface IBaseServiceReadOnly<T>
    {
        static IBaseService<T> Builder;

        Task<IEnumerable<T>> GetAsync();

        Task<T> GetAsync(int id);
    }
}
