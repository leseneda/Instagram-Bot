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
        static IGeoCode _geoCode;
        readonly string _urlEu = "https://eu1.locationiq.com/v1/search.php";
        readonly string _token = "9db25f38c0462a";

        private GeoCodeService()
        { 
        
        }

        public static IGeoCode Build()
        {
            _geoCode ??= new GeoCodeService();

            return _geoCode;
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
                    return JsonConvert
                        .DeserializeObject<IEnumerable<GeocodingEntity>>(response.Content.ReadAsStringAsync().Result);
                }
            }
            return null;
        }
    }
}
