using MeteoEmulator.Models;
using MeteoEmulator.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MeteoEmulator.Handlers
{
    public class DataSender
    {
        private HttpClient _httpClient;
        private HttpClient HttpClient => _httpClient ??= new HttpClient();

        private string _url;

        private readonly ILogger<DataSender> _logger;

        public DataSender(IOptions<Arguments> argOptions, ILogger<DataSender> logger)
        {
            _logger = logger;
            _url = argOptions.Value.Url.TrimEnd('/');
        }

        public async Task<bool> Send(MeteoDataPackage package, bool isDataWithNoise, CancellationToken token)
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

                var tmpUrl = _url + (isDataWithNoise ? "/noiseData" : "/data");

                _logger.LogDebug($"Sending data: {serializedForm} to url {tmpUrl}");

                var responseMessage = await HttpClient.PutAsync(tmpUrl, httpContent, token);

                if (responseMessage.StatusCode != HttpStatusCode.NoContent && responseMessage.StatusCode != HttpStatusCode.OK)
                {
                    var responseContent = await responseMessage.Content.ReadAsStringAsync(token);
                    _logger.LogError($"Response code is {responseMessage.StatusCode}\n" +
                                      $"Response message is {responseContent}");
                }

                return responseMessage.StatusCode is HttpStatusCode.NoContent or HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while sending");
                return false;
            }
        }
    }
}
