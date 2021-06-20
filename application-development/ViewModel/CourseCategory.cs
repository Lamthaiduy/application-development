using System;
using System.Collections.Generic;
using application_development.Models;

namespace application_development.ViewModel
{
    public class CourseCategory
    {
        public Course Course { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}