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
        private readonly IUrlService _urlService;

        public RouteService(IUrlService urlService)
        {
            _urlService = urlService;
        }
        
        public async Task<IEnumerable<Route>> GetMedicalRoutesAsync(string destination)
        {
            var resultText = await SearchLocation(_urlService.GetMedicalRouteUrl(), destination);
            return TransformTextResult(resultText);
        }

        public async Task<IEnumerable<Route>> GetPeopleRoutesAsync(string name)
        {
            var resultText = await SearchLocation(_urlService.GetPeopleRouteUrl(), name);
            return TransformTextResult(resultText);
        }

        public async Task<IEnumerable<Route>> GetPublicRoutesAsync(string destination)
        {
            var resultText = await SearchLocation(_urlService.GetPublicRouteUrl(), destination);
            return TransformTextResult(resultText);
        }

        private async Task<string> SearchLocation(string url, string destination)
        {
            HttpClient client = new HttpClient();

            var webserviceLocationQueryUrl = TransformUrl(url, destination);

            var response = await client.GetAsync(webserviceLocationQueryUrl);

            return await response.Content.ReadAsStringAsync();
        }

        private string TransformUrl(string url, string destination)
        {
            var webserviceLocationQueryUrl = url.Replace("{search}", destination);
            return webserviceLocationQueryUrl;
        }

        private IEnumerable<Route> TransformTextResult(string input)
        {
            return JsonConvert.DeserializeObject<IEnumerable<Route>>(input)
                ?? new List<Route>();
        }
    }
}
