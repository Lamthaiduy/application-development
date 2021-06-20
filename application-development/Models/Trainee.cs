using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace application_development.Models
{
    public class Trainee: ApplicationUser
    {
        public string TraineeName { get; set; }
        public int Age { get; set; }
        public string DateOfBirth { get; set; }
        public string Education { get; set; }
        public string Programming { get; set; }
        public string TOEICscore { get; set; }
        public string Exprience_details { get; set; }
        public string Department { get; set; }
    }
}