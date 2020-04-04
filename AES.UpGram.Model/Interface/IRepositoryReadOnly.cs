using System.Collections.Generic;
using UpSocial.UpGram.Domain.Entity;

namespace UpSocial.UpGram.Domain.Interface
{
    public interface IRepositoryReadOnly<T> where T : BaseEntity
    {
        T Select(int id);

        IEnumerable<T> SelectAll();
    }
}
