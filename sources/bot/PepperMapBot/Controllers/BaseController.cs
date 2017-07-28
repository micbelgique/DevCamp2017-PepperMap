using PepperMapBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PepperMapBot.Controllers
{
    /// <summary>
    /// Common class to implement controllers
    /// </summary>
    public abstract class BaseController : ApiController
    {
        private RouteService _routes;
        public RouteService Routes
        {
            get
            {
                if (_routes == null)
                    _routes = new RouteService();
                return _routes;
            }
        }
    }
}