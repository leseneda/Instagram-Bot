using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MeConecta.Gram.Service
{
    public class GeoCodeService : IGeoCode
    {
        private readonly string _urlEu = "https://eu1.locationiq.com/v1/search.php";
        private readonly string _token = "9db25f38c0462a";

        private GeoCodeService()
        { 
        
        }

        public static GeoCodeService Build()
        {
            return new GeoCodeService();
        }

        public IEnumerable<GeocodingEntity> SearchGeoCode(string query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_urlEu);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using var response = client.GetAsync($"?key={_token}&q={query}&format=json").Result;

                if (response.IsSuccessStatusCode)
                {
                    var dataString = response.Content.ReadAsStringAsync().Result;
                    
                    return JsonConvert.DeserializeObject<IEnumerable<GeocodingEntity>>(dataString);
                }
            }
            return null;
        }
    }
}
