using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Service;
using System;
using System.Threading.Tasks;

namespace MeConecta.Gram.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var geoCodeService = GeoCodeService.Build();
            //var geoCodeData = geoCodeService.SearchGeoCode("Rua vale de são martinho 14 sintra");

            var configBase = BaseServiceReadOnly<ConfigurationEntity>.Build();
            var configData = await configBase.GetAsync(1);

            var accountBase = BaseServiceReadOnly<AccountEntity>.Build();
            var accountData = accountBase.GetAsync().Result;

            foreach (var account in accountData)
            {
                configData.Account = account;

                var ret = new Runner().Execute(account.Id, 1, configData).Result;

                if (!ret)
                {

                }
            }
        }
    }
}
