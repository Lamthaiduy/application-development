using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace application_development.Models
{
    public class Trainer: ApplicationUser
    {
        public string TrainerName { get; set; }
        public string Type { get; set; }
        public string WorkPlace { get; set; }
    }
}