using System.Collections.Generic;
using UpSocial.UpGram.Domain.Entity;
using UpSocial.UpGram.Infra.Data.Repository;

namespace UpSocial.UpGram.Service
{
    public class BaseService<T> where T : BaseEntity
    {
        private readonly BaseDapperRepository<T> _repository = new BaseDapperRepository<T>();

        #region Changing

        public long Post(T obj) => _repository.Insert(obj);

        public bool Put(T obj) => _repository.Update(obj);

        public bool Delete(int id) => _repository.Remove(id);

        public bool Delete() => _repository.Remove();

        #endregion

        #region Reading

        public IEnumerable<T> Get() => _repository.SelectAll();

        public T Get(int id) => _repository.Select(id);

        #endregion

    }
}
