using application_development.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace application_development.ViewModel
{
    public class AssignTrainerCourse
    {
        public string TrainerId { get; set; }
        public int CourseId { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public TrainerCourse TrainerCourse { get; set; }

    }
}