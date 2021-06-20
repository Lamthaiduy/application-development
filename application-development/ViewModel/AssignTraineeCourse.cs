using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using application_development.Models;

namespace application_development.ViewModel
{
    public class AssignTraineeCourse
    {
        public string TraineeId { get; set; }
        public int CourseId { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public TraineeCourse TraineeCourse { get; set; }
    }
}