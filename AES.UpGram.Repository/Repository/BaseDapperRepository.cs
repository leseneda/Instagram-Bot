using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using UpSocial.UpGram.Domain.Entity;
using UpSocial.UpGram.Domain.Interface;

namespace UpSocial.UpGram.Infra.Data.Repository
{
	public class BaseDapperRepository<T> : SqlRepository, IRepository<T> where T : BaseEntity
	{
		#region Changing

		public long Insert(T entity)
		{
			using var conn = Connect();

			return conn.Insert(entity);
		}

		public bool Update(T entity)
		{
			using var conn = Connect();

			return conn.Update(entity);
		}

		public bool Remove(int id)
		{
			using var conn = Connect();

			var entity = conn.Get<T>(id);

			return conn.Delete(entity);
		}

		public bool Remove()
		{
			using var conn = Connect();

			return conn.DeleteAll<T>();
		}

		#endregion

		#region Reading

		public T Select(int id)
		{
			using var conn = Connect();

			return conn.Get<T>(id);
		}

		public IEnumerable<T> SelectAll()
		{
			using var conn = Connect();

			return conn.GetAll<T>();
		}

		#endregion
	}
}
