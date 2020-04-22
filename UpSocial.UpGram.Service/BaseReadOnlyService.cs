using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;
using MeConecta.Gram.Infra.Data.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeConecta.Gram.Service
{
    public class BaseReadOnlyService<T> : IBaseServiceReadOnly<T> where T : BaseEntity
    {
        private static BaseDapperRepository<T> _repository;

        private BaseReadOnlyService()
        {
        }

        protected BaseReadOnlyService(BaseDapperRepository<T> repository)
        {
            _repository = repository;
        }

        public static IBaseServiceReadOnly<T> Build()
        {
            _repository = new BaseDapperRepository<T>();

            return new BaseReadOnlyService<T>();
        }

        public async Task<IEnumerable<T>> GetAsync() => await _repository.SelectAllAsync();

        public async Task<T> GetAsync(long id) => await _repository.SelectAsync(id);

        public T GetFirst(Func<T, bool> predicate) => _repository.SelectFirst(predicate);

        public T GetLast(Func<T, bool> predicate) => _repository.SelectLast(predicate);

        public IEnumerable<T> GetWhere(Func<T, bool> predicate) => _repository.SelectWhere(predicate);
    }
}
