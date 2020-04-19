using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeConecta.Gram.Domain.Interface
{
    public interface IBaseServiceReadOnly<T>
    {
        static IBaseService<T> Builder;

        Task<T> GetAsync(long id);

        Task<IEnumerable<T>> GetAsync();
        
        IEnumerable<T> GetWhere(Func<T, bool> predicate);

        T GetFirst(Func<T, bool> predicate);

        T GetLast(Func<T, bool> predicate);
    }
}
