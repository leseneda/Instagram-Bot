using Dapper.Contrib.Extensions;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Infra.Data.Repository;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace MeConecta.Gram.Service
{
    public class BasePatternUnitOfWork : SqlRepository
    {
        List<BaseEntity> _entitiesPost;

        private BasePatternUnitOfWork()
        {
            _entitiesPost = new List<BaseEntity>();
        }

        public static BasePatternUnitOfWork Build()
        {
            return new BasePatternUnitOfWork();
        }

        public void Post<T>(T entity) where T : BaseEntity
        {
            _entitiesPost.Add(entity);
        }
        public async void Commit()
        {
            using TransactionScope scope = new TransactionScope();
            //using var conn = Connect();

            foreach (var entity in _entitiesPost)
            {
                //var repository = new BaseDapperRepository<ConfigurationEntity>();
                //await repository.InsertAsync((ConfigurationEntity)entity);

   
                //conn.Insert((ConfigurationEntity)entity);

                //var _repository = new BaseDapperRepository<BaseEntity>();
                //await _repository.InsertAsync(entity);
            }

            scope.Complete();
        }
    }
}
