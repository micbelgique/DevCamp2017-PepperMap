using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PepperMap.Infrastructure.Models;
using System.Net.Http;
using Newtonsoft.Json;
using PepperMap.Infrastructure.Interfaces;

namespace PepperMap.Infrastructure.Services
{
    [Serializable]
    public class RouteService : IRouteService
    {
        private readonly ISettingService _settingService;

        public RouteService(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public async Task<IEnumerable<Route>> GetRoutesAsync(string destination)
        {
            var resultText = await SearchLocation(destination);
            return TransformTextResult(resultText);
        }

        private async Task<string> SearchLocation(string destination)
        {
            HttpClient client = new HttpClient();

            var webserviceLocationQueryUrl = GetWebserviceUrl(destination);

            var response = await client.GetAsync(webserviceLocationQueryUrl);

            return await response.Content.ReadAsStringAsync();
        }

        private string GetWebserviceUrl(string destination)
        {
            var webserviceLocationQueryUrl = _settingService
                .GetSetting("webserviceLocationQueryUrl")
                .Replace("param", destination);
            return webserviceLocationQueryUrl;
        }

        private IEnumerable<Route> TransformTextResult(string input)
        {
            return JsonConvert.DeserializeObject<IEnumerable<Route>>(input);
        }
    }
}
