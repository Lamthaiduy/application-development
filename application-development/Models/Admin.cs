using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace application_development.Models
{
    public class Admin: ApplicationUser
    {
        public string AdminName { get; set; }
    }
}