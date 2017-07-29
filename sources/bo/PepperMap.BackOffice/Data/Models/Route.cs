using System;
using System.Collections.Generic;
using System.Text;

namespace PepperMap.BackOffice.Data.Models
{
   public class Route
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Indicator { get; set; }
        public RouteFlag Flag { get; set; }
    }

    [Flags]
    public enum RouteFlag
    {
        All,
        Public,
        Medical
    }
}
