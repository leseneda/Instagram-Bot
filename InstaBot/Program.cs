using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstaSharper;
using InstaSharper.Classes;
using InstaSharper.API;
using InstaSharper.API.Builder;
using InstaSharper.Logger;
using InstaSharper.Classes.Models;

namespace InstaBot
{
    public class Program
    {
        private static string username = "acontabate";
        private static string password = "balacal@16";

        private static UserSessionData user;

        private static IInstaApi api;

        static void Main(string[] args)
        {
            user = new UserSessionData()
            {
                UserName = username,
                Password = password,
            };

            bool isConnected = (Connect().Result);

            if (isConnected)
            {
                UnFollowUser(user.UserName);
            }
            
            
            
            //Login();
     
            Console.Read();
        }

        public static async Task<bool> Connect()
        {
            api = InstaApiBuilder.CreateBuilder()
                .SetUser(user)
                .UseLogger(new DebugLogger(LogLevel.Exceptions))
                .SetRequestDelay(TimeSpan.FromSeconds(1))
                .Build();

            var loginRequest = await api.LoginAsync();

            return loginRequest.Succeeded;
        }

        public static async void Login()
        {
            api = InstaApiBuilder.CreateBuilder()
                .SetUser(user)
                .UseLogger(new DebugLogger(LogLevel.Exceptions))
                .SetRequestDelay(TimeSpan.FromSeconds(1))
                .Build();

            var loginRequest = await api.LoginAsync();

            if (loginRequest.Succeeded)
            {
                string nextId = string.Empty; // QVFEWUJOQUZjb1F5SllreWJndXUzVi11ZW5EbElXM0NYRE1DWUZBSzhPWVcwS0VQNDF5QWxiXzBfbklXeDJUdjRfSEZHT24zZEN2cEw0YUlmRFEzNTZSTQ==

                do
                {
                    nextId = await FollowUser("mirna_economirna", nextId);

                } while (!string.IsNullOrEmpty(nextId));
            }
            else
            {
                Console.Write("NO");
            }
        }

        public static async void UnFollowUser(string accountName)
        {
            var userFollowinglist = await api.GetUserFollowingAsync(accountName, PaginationParameters.MaxPagesToLoad(5));
            var userFollowerslist = await api.GetUserFollowersAsync(accountName, PaginationParameters.MaxPagesToLoad(5));

            var userNotFollowinglist = (from following in userFollowinglist.Value.ToList()
                                        join followers in userFollowerslist.Value.ToList() 
                                        on following.Pk equals followers.Pk into noFollowing
                                        from crossData in noFollowing.DefaultIfEmpty()
                                        where ReferenceEquals(null, crossData)
                                        select crossData);

            foreach (InstaUserShort user in userNotFollowinglist)
            {
                await api.UnFollowUserAsync(user.Pk);
            }
        }

        public static async Task<string> FollowUser(string userToScrape, string fromNextId = null)
        {
            int maxPageToLoad = 1;

            var param = (!string.IsNullOrEmpty(fromNextId) ? 
                PaginationParameters.MaxPagesToLoad(maxPageToLoad).StartFromId(fromNextId): 
                PaginationParameters.MaxPagesToLoad(maxPageToLoad));

            var userMainList = await api.GetUserFollowersAsync(userToScrape, param);

            string nextId = userMainList.Value?.NextId ?? string.Empty;

            foreach (InstaUserShort user in userMainList.Value)
            {
                await api.FollowUserAsync(user.Pk);
            }

            return nextId;
        }

        //async Task<List<InstaUserShort>> GetFollowersList(string userToScrape, PaginationParameters parameters, List<InstaUserShort> followers)
        //{
        //    if (followers == null)
        //        followers = new List<InstaUserShort>();
        //    // load more followers
        //    Console.WriteLine($"Loaded so far: {followers.Count}, loading more");
        //    var result = await api.GetUserFollowersAsync(userToScrape, parameters);

        //    // merge results
        //    if (result.Value != null)
        //        followers = result.Value.Union(followers).ToList();
        //    if (result.Succeeded)
        //        return followers;

        //    // prepare nex id
        //    var nextId = result.Value?.NextId ?? parameters.NextId;

        //    // setup some delay
        //    var delay = TimeSpan.FromSeconds(new Random(DateTime.Now.Millisecond).Next(60, 120));
        //    if (result.Info.ResponseType == ResponseType.RequestsLimit)
        //        delay = TimeSpan.FromSeconds(new Random(DateTime.Now.Millisecond).Next(120, 360));
        //    Console.WriteLine($"Not able to load full list of followers, retry in {delay.TotalSeconds} seconds");
        //    await Task.Delay(delay);
        //    return await GetFollowersList(userToScrape, parameters.StartFromId(nextId), followers);
        //}

    }

}
