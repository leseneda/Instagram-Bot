using System;
using System.Net.Http;

namespace AES.UpGram.Core.Utils
{
    public class AccessKey
    {
        public bool IsValidKeyDate
        {
            get { return CompareKeyDate(); }
        }

        #region Private

        private DateTime? GetInternetDateTime()
        {
            try
            {
                using var client = new HttpClient();
                
                var result = client.GetAsync("https://google.com", HttpCompletionOption.ResponseHeadersRead).Result;

                return result.Headers.Date.Value.DateTime;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private bool CompareKeyDate()
        {
            var internetDate = GetInternetDateTime();

            return (internetDate.HasValue ? (DateTime.Compare(internetDate.Value.Date, DateTime.UtcNow.Date) == 0) : false);
        }

        #endregion



    }
}
