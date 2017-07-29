using PepperMap.Infrastructure.Interfaces;

namespace PepperMap.Infrastructure.Services
{
    public class UrlService : IUrlService
    {
        private readonly ISettingService _settingService;

        public UrlService(ISettingService settingService)
        {
            _settingService = settingService;
        }


        public string GetPublicRouteUrl()
        {
            return _settingService.GetSetting("WebservicePublicLocationQueryUrl");
        }

        public string GetMedicalRouteUrl()
        {
            return _settingService.GetSetting("WebserviceMedicalLocationQueryUrl");
        }

        public string GetPeopleRouteUrl()
        {
            return _settingService.GetSetting("WebservicePeopleLocationQueryUrl");

        }
    }
}
