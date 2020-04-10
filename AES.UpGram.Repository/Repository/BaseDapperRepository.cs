using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;

namespace MeConecta.Gram.Infra.Data.Repository
{
    public class BaseDapperRepository<T> : SqlRepository, IRepository<T> where T : BaseEntity
    {
        #region Changing

        public async Task<long> InsertAsync(T entity)
        {
            using var conn = Connect();

            return await conn.InsertAsync(entity);
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            using var conn = Connect();

            return await conn.UpdateAsync(entity);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            using var conn = Connect();

            var entity = await conn.GetAsync<T>(id);

            return await conn.DeleteAsync(entity);
        }

        public async Task<bool> RemoveAsync()
        {
            using var conn = Connect();

            return await conn.DeleteAllAsync<T>();
        }

        #endregion

        #region Reading

        public async Task<T> SelectAsync(int id)
        {
            using var conn = Connect();

            return await conn.GetAsync<T>(id);
        }

        public async Task<IEnumerable<T>> SelectAllAsync()
        {
            using var conn = Connect();

            return await conn.GetAllAsync<T>();
        }

        #endregion
    }
}
