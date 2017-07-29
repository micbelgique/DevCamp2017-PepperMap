using System;
using System.Collections.Generic;
using System.Text;

namespace PepperMap.DomainLayer.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Title { get; set; }
        public bool IsPatient { get; set; }
        public bool IsStaff { get; set; }
        public string Service { get; set; }
        public int LocationId { get; set; }
    }
}
