using Dapper.Contrib.Extensions;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<bool> RemoveAsync(long id)
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

        public async Task<T> SelectAsync(long id)
        {
            using var conn = Connect();

            return await conn.GetAsync<T>(id);
        }

        public async Task<IEnumerable<T>> SelectAllAsync()
        {
            using var conn = Connect();

            return await conn.GetAllAsync<T>();
        }

        public IEnumerable<T> SelectWhere(Func<T, bool> predicate)
        {
            using var conn = Connect();

            return conn.GetAll<T>()
                .Where(predicate);
        }

        public T SelectFirst(Func<T, bool> predicate)
        {
            using var conn = Connect();
            
            return conn.GetAll<T>().FirstOrDefault(predicate);
        }

        public T SelectLast(Func<T, bool> predicate)
        {
            using var conn = Connect();

            return conn.GetAll<T>().LastOrDefault(predicate);
        }

        #endregion
    }
}
