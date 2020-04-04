using System;
using UpSocial.UpGram.Core;
using UpSocial.UpGram.Domain.Entity;
using UpSocial.UpGram.Service;

namespace AES.UpGram.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Account Detail

            var service = new BaseService<AccountEntity>();
            var account = service.Get(1);
            var accounts = service.Get();

            //var add = service.Post(new AccountEntity()
            //{
            //    Name = "carolinaseneda",
            //    Password = "28vegana",
            //    IsActive = true,
            //    CreatedOn = DateTime.UtcNow
            //});

            //var delete = service.Delete(3);

            //var update = service.Put(new AccountEntity()
            //{
            //    Id = 2,
            //    Name = "carolinaseneda",
            //    Password = "28vegana",
            //    IsActive = true,
            //    CreatedOn = DateTime.UtcNow
            //});




            var connector = new Connector(new AccountEntity()
            {
                Name = account.Name,
                Password = account.Password
            });

            #endregion

            var login = connector.Login().Result;

            if (login)
            {
                


                //string fromNextId = "QVFBZ0RGZVZFRm9vclBvcTJOWHRaOEQtUUp2bDBfYkdtWkZXVmktSEVlNDdzSWJBYzRjNGYybnRRaTZqeGdHZm1mZUU3SWhKT3lmZTUzUXFEM0dFSG9GMQ=="; // alana_rox

                //do
                //{
                //    fromNextId = connector.User.Value.Follow("alana_rox", fromNextId).Result;

                //} while (!string.IsNullOrEmpty(fromNextId));

                //var unfollowed = connector.User.Value.UnFollow().Result;


            }




            connector.Logout();
        }
    }
}
