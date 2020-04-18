using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;
using MeConecta.Gram.Infra.Data.Repository;
using System.Threading.Tasks;

namespace MeConecta.Gram.Service
{
    public class BaseService<T> : BaseServiceReadOnly<T>, IBaseService<T> where T : BaseEntity
    {
        private static BaseDapperRepository<T> _repository;

        private BaseService()
        {

        }

        public static new IBaseService<T> Build()
        {
            _repository = new BaseDapperRepository<T>();

            return new BaseService<T>();
        }

        #region Changing

        public async Task<long> PostAsync(T entity) => await _repository.InsertAsync(entity);

        public async Task<bool> PutAsync(T entity) => await _repository.UpdateAsync(entity);

        public async Task<bool> DeleteAsync(int id) => await _repository.RemoveAsync(id);

        public async Task<bool> DeleteAsync() => await _repository.RemoveAsync();

        #endregion

        #region Reading

        //public async Task<IEnumerable<T>> GetAsync() => await _repository.SelectAllAsync();

        //public async Task<T> GetAsync(int id) => await _repository.SelectAsync(id);

        #endregion
    }
}
