using AES.UpGram.Repository;
using Dapper;
using UpSocial.UpGram.Domain.Entity;

namespace UpSocial.UpGram.Repository
{
    public class DapperAccountRepository : SqlRepository//, IAccountRepository
    {
        public AccountEntity GetById(int accountId)
        {
            using var conn = Connect();

            return conn.QueryFirstOrDefault<AccountEntity>("SELECT Id, Name, Password, IsActive, CreatedOn FROM Account WHERE Id = @AccountId", 
                new { AccountId = accountId });
        }
    }
}
