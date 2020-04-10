using System.Collections.Generic;
using System.Threading.Tasks;
using UpSocial.UpGram.Domain.Entity;
using UpSocial.UpGram.Domain.Interface;
using UpSocial.UpGram.Infra.Data.Repository;

namespace UpSocial.UpGram.Service
{
    public class BaseServiceReadOnly<T> : IBaseServiceReadOnly<T> where T : BaseEntity
    {
        private static BaseDapperRepository<T> _repository;

        private BaseServiceReadOnly()
        {
        }

        public static IBaseServiceReadOnly<T> Builder()
        {
            _repository = new BaseDapperRepository<T>();

            return new BaseServiceReadOnly<T>();
        }

        public async Task<IEnumerable<T>> GetAsync() => await _repository.SelectAllAsync();

        public async Task<T> GetAsync(int id) => await _repository.SelectAsync(id);

    }
}
