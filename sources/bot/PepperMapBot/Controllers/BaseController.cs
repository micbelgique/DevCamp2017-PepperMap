using System.Web.Http;
using System.Web.Mvc;
using PepperMap.Infrastructure.Interfaces;

namespace PepperMapBot.Controllers
{
    /// <summary>
    /// Common class to implement controllers
    /// </summary>
    public abstract class BaseController : ApiController
    {
        protected BaseController()
        {
            RouteService = DependencyResolver.Current.GetService<IRouteService>();
        }
        public IRouteService RouteService { get; protected set; }
    }
}