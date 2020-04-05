using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using UpSocial.UpGram.Domain.Entity;
using UpSocial.UpGram.Domain.Interface;

namespace UpSocial.UpGram.Infra.Data.Repository
{
    public class BaseDapperRepository<T> : SqlRepository, IRepository<T> where T : BaseEntity
    {
        #region Changing

        public async Task<long> InsertAsync(T entity)
        {
            using var conn = Connect();

            return await conn.InsertAsync(entity);
        }

        public Task<bool> UpdateAsync(T entity)
        {
            using var conn = Connect();

            return conn.UpdateAsync(entity);
        }

        public Task<bool> RemoveAsync(int id)
        {
            using var conn = Connect();

            var entity = conn.GetAsync<T>(id);

            return conn.DeleteAsync(entity);
        }

        public Task<bool> RemoveAsync()
        {
            using var conn = Connect();

            return conn.DeleteAllAsync<T>();
        }

        #endregion

        #region Reading

        public Task<T> SelectAsync(int id)
        {
            using var conn = Connect();

            return conn.GetAsync<T>(id);
        }

        public Task<IEnumerable<T>> SelectAllAsync()
        {
            using var conn = Connect();

            return conn.GetAllAsync<T>();
        }

        #endregion
    }
}
