using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeConecta.Gram.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var geoCodeService = GeoCodeService.Builder();
            //var geoCodeData = geoCodeService.SearchGeoCode("Rua vale de são martinho 14 sintra");

            var configBase = BaseServiceReadOnly<ConfigurationEntity>.Build();
            var configData = await configBase.GetAsync(1);

            var accountBase = BaseServiceReadOnly<AccountEntity>.Build();
            var accountData = await accountBase.GetAsync(1);

            //var taskList = new List<Task>();

            configData.Account = accountData;

            var ret = new Runner().Execute(accountData.Id, 3, configData).Result;

            //foreach (var account in accountData.Where(cmp => cmp.Id == 1))
            //{
            //    configData.Account = account;

            //    Task task = Task.Run(() => new Runner().Execute(account.Id, 1, configData));
            //    taskList.Add(task);
            //}

            //Task.WaitAll(taskList.ToArray());
        }
    }
}
