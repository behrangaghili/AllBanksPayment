using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace AspNetCore21Payment.Models.Pay
{
    public class WebApiService
    {
        #region سازنده پیش فرض
        public WebApiService()
        {

        }
        #endregion

        #region GetData
        public static async Task<T> GetData<T>(string url)
        {
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                System.Net.Http.HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    dynamic jsonDate = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data);

                    return jsonDate;
                }
            }

            return default(T);
        }
        #endregion

        #region PostData
        public static async Task<T> PostData<T>(string url, object dataSend)
        {
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(dataSend);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                System.Net.Http.HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    dynamic jsonDate = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data);

                    return jsonDate;
                }
            }

            return default(T);
        }
        #endregion
    }
}