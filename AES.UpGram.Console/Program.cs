using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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
            var accountData = await accountBase.GetAsync();
            
            foreach (var account in accountData)
            {
                bool ret = new Runner().Execute(account.Id, 1, configData).Result;
            }
        }
    }
}
