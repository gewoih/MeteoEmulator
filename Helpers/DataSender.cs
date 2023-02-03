using MeteoEmulator.Models;
using MeteoEmulator.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MeteoEmulator.Helpers
{
    internal class DataSender
    {
        private HttpClient _httpClient;
        private HttpClient HttpClient => _httpClient ??= new HttpClient();

        public DataSender()
        {
        }

        public async Task<bool> Send(MeteoDataPackage package, string url, CancellationToken token)
        {
            try
            {
                var serializedForm = JsonConvert.SerializeObject(package, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
                    DateParseHandling = DateParseHandling.None,
                    ContractResolver = new CustomCamelCasePropertyNamesContractResolver() { NamingStrategy = new CamelCaseNamingStrategy { ProcessDictionaryKeys = true } }
                });

                using HttpContent httpContent = new StringContent(serializedForm, Encoding.UTF8, "application/json");

                Console.WriteLine($"Sending data: {serializedForm} to url {url}");

                var responseMessage = await HttpClient.PutAsync(url, httpContent, token);

                if (responseMessage.StatusCode != HttpStatusCode.NoContent && responseMessage.StatusCode != HttpStatusCode.OK)
                {
                    var responseContent = await responseMessage.Content.ReadAsStringAsync(token);
                    Console.WriteLine($"Response code is {responseMessage.StatusCode}\n" +
                                      $"Response message is {responseContent}");
                }

                return responseMessage.StatusCode is HttpStatusCode.NoContent or HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
