using System.Collections.Generic;
using System.Threading.Tasks;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;
using MeConecta.Gram.Infra.Data.Repository;

namespace MeConecta.Gram.Service
{
    public class BaseServiceReadOnly<T> : IBaseServiceReadOnly<T> where T : BaseEntity
    {
        private static BaseDapperRepository<T> _repository;

        private BaseServiceReadOnly()
        {
        }

        public static IBaseServiceReadOnly<T> Build()
        {
            _repository = new BaseDapperRepository<T>();

            return new BaseServiceReadOnly<T>();
        }

        public async Task<IEnumerable<T>> GetAsync() => await _repository.SelectAllAsync();

        public async Task<T> GetAsync(int id) => await _repository.SelectAsync(id);

    }
}
