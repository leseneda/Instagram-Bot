using System.Collections.Generic;
using UpSocial.UpGram.Domain.Entity;
using UpSocial.UpGram.Infra.Data.Repository;

namespace UpSocial.UpGram.Service
{
    public class BaseService<T> where T : BaseEntity
    {
        private readonly BaseDapperRepository<T> _repository = new BaseDapperRepository<T>();

        public T Post(T obj)
        {
            _repository.Insert(obj);
            return obj;
        }

        public T Put(T obj)
        {
            _repository.Update(obj);
            return obj;
        }

        public void Delete(int id) => _repository.Remove(id);

        public IEnumerable<T> Get() => _repository.SelectAll();

        public T Get(int id) => _repository.Select(id);
    }
}
