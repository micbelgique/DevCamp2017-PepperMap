using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PepperMapBot.Services.Entities
{
    public class Route
    {
        public string Name { get; set; }

        public string Number { get; set; }

        public override string ToString()
        {
            return $"{Name}' - '{Number}";
        }
    }
}