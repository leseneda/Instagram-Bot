using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MeConecta.Gram.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var geoCodeService = GeoCodeService.Build();
            //var geoCodeData = geoCodeService.SearchGeoCode("Rua vale de são martinho 14 sintra");

            //AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) =>
            //{
            //    // Global handler exception
            //    //Debug.WriteLine(eventArgs.Exception.ToString());
            //};

            var configBase = BaseReadOnlyService<ConfigurationEntity>.Build();
            var configData = await configBase.GetAsync(1);

            var accountBase = BaseReadOnlyService<AccountEntity>.Build();
            var accountData = accountBase.GetAsync().Result;

            foreach (var account in accountData)
            {
                configData.Account = account;

                var ret = new Runner().Execute(account.Id, 2, configData).Result;

                if (!ret)
                {

                }
            }
        }
    }
}
