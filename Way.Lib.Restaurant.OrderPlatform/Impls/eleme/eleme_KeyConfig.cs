using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Way.Lib.Restaurant.OrderPlatform.Impls.eleme
{
    internal class eleme_KeyConfig : KeyConfig
    {
#if DEBUG
        string ServerUrl = "https://open-api-sandbox.shop.ele.me";
#else
        string ServerUrl = "https://open-api.shop.ele.me";
#endif
        string _token;

        public string Key = "wYO4C8ZLzB";
        public string Secret = "8e81ef8b73593d6b24e0cd9f11cbe3132d11ab8e";

        /// <summary>
        /// 获取访问令牌
        /// </summary>
        internal void GetToken()
        {
            if (_token == null)
            {
                //HttpClientHandler clienthandler = new HttpClientHandler();

                HttpClient client = new HttpClient();
                string base64 = Convert.ToBase64String( System.Text.Encoding.UTF8.GetBytes($"{Key}:{Secret}"));
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {base64}");
                //client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");

                var content = new FormUrlEncodedContent(new Dictionary<string, string>()
               {
                   {"grant_type", "client_credentials"}
               });
                var task = client.PostAsync($"{ServerUrl}/token", content);
                task.Wait();
                var response = task.Result;

                var task2 = response.Content.ReadAsStringAsync();
                task2.Wait();
                _token = task2.Result;
            }
        }
    }
}
