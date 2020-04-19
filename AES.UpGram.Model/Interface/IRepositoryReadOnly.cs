using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeConecta.Gram.Domain.Entity;

namespace MeConecta.Gram.Domain.Interface
{
    public interface IRepositoryReadOnly<T> where T : BaseEntity
    {
        Task<T> SelectAsync(long id);

        Task<IEnumerable<T>> SelectAllAsync();

        IEnumerable<T> SelectWhere(Func<T, bool> predicate);
        
        T SelectFirst(Func<T, bool> predicate);

        T SelectLast(Func<T, bool> predicate);
    }
}
