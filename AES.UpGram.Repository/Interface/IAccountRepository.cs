using UpSocial.UpGram.Domain.Entity;

namespace AES.UpGram.Repository
{
    public interface IAccountRepository
    {
        public AccountEntity GetById(int accountId);
    }
}
