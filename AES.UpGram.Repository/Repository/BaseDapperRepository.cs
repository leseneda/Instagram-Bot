using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using UpSocial.UpGram.Domain.Entity;
using UpSocial.UpGram.Domain.Interface;

namespace UpSocial.UpGram.Infra.Data.Repository
{
	public class BaseDapperRepository<T> : SqlRepository, IRepository<T> where T : BaseEntity
	{
		#region Changing

		public BaseDapperRepository()
		{
		}

		public void Insert(T entity)
		{
			using var conn = Connect();

			conn.Insert(entity);
		}

		public void Update(T entity)
		{
			using var conn = Connect();

			conn.Update(entity);
		}

		public void Remove(int id)
		{
			using var conn = Connect();

			var entity = conn.Get<T>(id);

			conn.Update(entity);
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
