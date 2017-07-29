namespace PepperMap.Infrastructure.Interfaces
{
    public interface IUrlService
    {
        string GetMedicalRouteUrl();
        string GetPeopleRouteUrl();
        string GetPublicRouteUrl();
        string GetRouteNumberUrl();
    }
}